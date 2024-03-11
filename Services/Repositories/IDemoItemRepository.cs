using Domain;
using Services.DTOs.DemoItemDTOs;

namespace Services.Repositories
{
    public interface IDemoItemRepository : IRepository<DemoItem>
    {
        Task<List<DemoItemDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
