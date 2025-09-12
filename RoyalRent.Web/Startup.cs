using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoyalRent.Application.Extensions;
using RoyalRent.Infrastructure.Database;
using RoyalRent.Infrastructure.Extensions;
using RoyalRent.Infrastructure.Persistence;
using RoyalRent.Presentation.Extensions;
using RoyalRent.Presentation.Middlewares;
using RoyalRent.Web.Extensions;
using Scrutor;
using Serilog;
using Stripe;

namespace RoyalRent.Web;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;
    public readonly ILoggerFactory factory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        StripeConfiguration.ApiKey = Configuration.GetValue<string>("Stripe:SecretKey") ??
                                     throw new ArgumentException("Stripe Key must be set to initiate application");

        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

        var mainDbName = Configuration.GetValue<string>("DbNames:MainDatabaseName") ??
                         throw new ArgumentException("Database must be set to initiate application");

        services.AddDbContext<ApiDbContext>(builder =>
        {
            builder.UseNpgsql(Configuration.GetConnectionString(mainDbName), pgAction =>
            {
                pgAction.EnableRetryOnFailure(3);
                pgAction.CommandTimeout(30);
            });

            builder.UseLoggerFactory(factory);

            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging();
        });

        services.AddDbContextFactory<ApiDbContext>(lifetime: ServiceLifetime.Scoped);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(applicationAssembly);
        });

        services.Scan(selector => selector.FromAssemblies(
                Infrastructure.AssemblyReference.Assembly
            )
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services
            .AddApplicationCollection()
            .AddInfrastructureCollection(Configuration)
            .AddPresentationCollection();

        services.AddGlobalErrorException();

        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
            {
                policy.WithOrigins("http://localhost:5173");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
            var filePath = Path.Combine(System.AppContext.BaseDirectory, "RoyalRent.Presentation.xml");
            config.IncludeXmlComments(filePath);
            config.UseInlineDefinitionsForEnums();
        });

        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]!))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var tokenFromCookie = context.HttpContext.Request.Cookies["access_token"];
                    if (string.IsNullOrEmpty(tokenFromCookie))
                    {
                        context.NoResult();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }

                    context.Token = tokenFromCookie;
                    return Task.CompletedTask;
                }
            };
        }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
            options.Cookie.IsEssential = true;
        });
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1");
                c.InjectStylesheet("/swagger-ui/SwaggerDarkMode.css");
            });

            app.UseStaticFiles();
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseRouting();

        app.UseSession();

        app.UseGlobalErrorHandler();


        // Adding Middlewares and ordering them by their urgency
        app.UseMiddleware<ResponseMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapErrorHandling();
        });
    }
}
