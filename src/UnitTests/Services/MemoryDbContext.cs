using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Services
{
    public class MemoryDbContext(string dbName) : AppDbContext
    {
        private readonly string _dbName = dbName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_dbName);
        }
    }
}
