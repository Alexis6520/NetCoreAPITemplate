using Domain;
using Services.DTOs;

namespace Services.Repositories
{
    public interface IDemoItemRepository : IRepository<DemoItem>
    {
        Task<List<DemoItemDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
