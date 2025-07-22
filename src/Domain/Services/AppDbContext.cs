using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public abstract class AppDbContext : DbContext
    {
        public DbSet<Donut> Donuts { get; set; }
    }
}
