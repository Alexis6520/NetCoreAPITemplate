using Services.Repositories;

namespace Services
{
    public interface IUnitOfWork
    {
        IDemoItemRepository DemoItems { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
