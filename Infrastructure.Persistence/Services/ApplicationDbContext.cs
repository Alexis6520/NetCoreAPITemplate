using Domain;
using Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Services
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<DemoItem> DemoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = new DemoItemConfig(modelBuilder.Entity<DemoItem>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
