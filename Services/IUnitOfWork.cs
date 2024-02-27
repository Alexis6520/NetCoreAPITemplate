using Services.Repositories;

namespace Services
{
    public interface IUnitOfWork
    {
        IDemoItemRepository Items { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
