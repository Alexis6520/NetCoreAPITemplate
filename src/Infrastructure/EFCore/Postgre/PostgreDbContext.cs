using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EFCore.Postgre
{
    public class PostgreDbContext(IConfiguration configuration) : AppDbContext
    {
        private readonly IConfiguration _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var validNamespaces = new List<string>
            {
                "Infrastructure.EFCore.Configuration",
                "Infrastructure.EFCore.Postgre.Configuration"
            };

            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                x => x.Namespace is not null && validNamespaces.Contains(x.Namespace));
        }
    }
}
