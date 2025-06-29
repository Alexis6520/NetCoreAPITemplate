using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EFCore.SqlServer
{
    /// <summary>
    /// Implementación de sesión de base de datos para Microsoft SQL Server.
    /// </summary>
    /// <param name="configuration"></param>
    public class SqlServerDbContext(IConfiguration configuration) : ApplicationDbContext
    {
        private readonly IConfiguration _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
        }
    }
}
