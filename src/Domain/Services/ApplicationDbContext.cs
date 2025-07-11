using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public abstract class ApplicationDbContext : DbContext
    {
        public DbSet<Donut> Donuts { get; set; }
    }
}
