using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoyalRent.Infrastructure.Database;
using Scrutor;
using Serilog;

namespace RoyalRent.Web;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

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

        var presentationAssembly = Presentation.AssemblyReference.Assembly;

        services.AddControllers().AddApplicationPart(presentationAssembly);

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
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

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
