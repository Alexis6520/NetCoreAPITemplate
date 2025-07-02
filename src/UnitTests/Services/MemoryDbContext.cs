using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Services
{
    /// <summary>
    /// Implemenatación de sesión de la base de datos en memoria para pruebas unitarias.
    /// </summary>
    /// <param name="dbName"></param>
    public class MemoryDbContext(string dbName) : ApplicationDbContext
    {
        private readonly string _dbName = dbName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_dbName);
        }
    }
}
