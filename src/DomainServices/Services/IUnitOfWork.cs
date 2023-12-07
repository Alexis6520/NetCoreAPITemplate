using DomainServices.Repositories;

namespace DomainServices.Services
{
    public interface IUnitOfWork
    {
        IDemoItemRepository DemoItems { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
