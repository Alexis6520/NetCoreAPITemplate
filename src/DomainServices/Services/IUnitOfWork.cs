using DomainServices.Repositories;

namespace DomainServices.Services
{
    public interface IUnitOfWork
    {
        IDemoItemRepository DemoItems { get; }

        /// <summary>
        /// Ejecuta todos los cambios pendientes
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
