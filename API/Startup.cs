using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public static class Startup
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app, CancellationToken cancellationToken = default)
        {
            using var scope = app.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
