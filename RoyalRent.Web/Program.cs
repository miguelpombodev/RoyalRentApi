using Microsoft.EntityFrameworkCore;
using RoyalRent.Infrastructure.Database;
using Serilog;

namespace RoyalRent.Web;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var webHost = CreateHostBuilder(args);

        webHost.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        var buildedHost = webHost.Build();

        await ApplyMigrations(buildedHost.Services);

        await buildedHost.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

    private static async Task ApplyMigrations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
