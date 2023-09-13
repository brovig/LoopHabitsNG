using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Repository;
public static class MigrationsManager
{
    public static void MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            using var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
            if (appContext.Database.GetPendingMigrations().Any())
                appContext.Database.Migrate();
        }
    }    
}
