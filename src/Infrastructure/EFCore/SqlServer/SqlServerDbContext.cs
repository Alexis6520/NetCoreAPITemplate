using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EFCore.SqlServer
{
    public class SqlServerDbContext(IConfiguration configuration) : ApplicationDbContext
    {
        private readonly IConfiguration _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var configNamespaces = new List<string>
            {
                "Infrastructure.EFCore.Configuration",
                "Infrastructure.EFCore.SqlServer.Configuration"
            };

            var assembly = typeof(SqlServerDbContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(
                assembly,
                x => x.Namespace is not null && configNamespaces.Contains(x.Namespace));
        }
    }
}
