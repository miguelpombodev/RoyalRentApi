using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoyalRent.Infrastructure.Database;
using Serilog;

namespace RoyalRent.Web;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; set; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
        
        services.AddControllers().AddApplicationPart(presentationAssembly);

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1"});
        });

        services.AddDbContext<ApiDbContext>(builder =>
        {
            builder.UseNpgsql(Configuration.GetConnectionString("PSQLDatabase"), pgAction =>
            {
                pgAction.EnableRetryOnFailure(3);
                pgAction.CommandTimeout(30);
            });

            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging();
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
        }

        // app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}