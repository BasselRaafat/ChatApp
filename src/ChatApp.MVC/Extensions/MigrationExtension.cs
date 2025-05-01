using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.MVC.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Erorr Happened While Updating Database");
        }
    }
}
