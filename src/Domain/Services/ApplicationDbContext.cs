using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    /// <summary>
    /// Representa una sesión de la base de datos para la aplicación.
    /// </summary>
    public abstract class ApplicationDbContext : DbContext
    {
        public DbSet<Donut> Donuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicamos la configuración general para todas las implementaciones
            ApplyGlobalConfiguration(modelBuilder);

            // Aplicamos la configuración específica para la implementación actual
            ApplySpecificConfiguration(modelBuilder);
        }

        private void ApplyGlobalConfiguration(ModelBuilder modelBuilder)
        {
            Type contextType = GetType();
            if (string.IsNullOrEmpty(contextType.Namespace)) return;
            string globalConfigNamespace = string.Join('.', contextType.Namespace.Split('.')[..-1]);
            globalConfigNamespace += ".Configuration";

            modelBuilder.ApplyConfigurationsFromAssembly(
                contextType.Assembly,
                t => t.Namespace is not null && t.Namespace == globalConfigNamespace);
        }

        private void ApplySpecificConfiguration(ModelBuilder modelBuilder)
        {
            Type contextType = GetType();
            if (string.IsNullOrEmpty(contextType.Namespace)) return;
            string configNamespace = contextType.Namespace + ".Configuration";

            modelBuilder.ApplyConfigurationsFromAssembly(
                contextType.Assembly,
                t => t.Namespace is not null && t.Namespace == configNamespace);
        }
    }
}
