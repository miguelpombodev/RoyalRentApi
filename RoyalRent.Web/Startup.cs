using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoyalRent.Application.Extensions;
using RoyalRent.Infrastructure.Database;
using RoyalRent.Infrastructure.Extensions;
using RoyalRent.Presentation.Extensions;
using Scrutor;
using Serilog;

namespace RoyalRent.Web;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;
    public readonly ILoggerFactory factory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApiDbContext>(builder =>
        {
            builder.UseNpgsql(Configuration.GetConnectionString("PostgreSQLDatabase"), pgAction =>
            {
                pgAction.EnableRetryOnFailure(3);
                pgAction.CommandTimeout(30);
            });

            builder.UseLoggerFactory(factory);

            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging();
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
            .AddAplicationCollection()
            .AddInfrastructureCollection(Configuration)
            .AddPresentationCollection();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
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
        });
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
