using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Donut> Donuts { get; set; }
}
