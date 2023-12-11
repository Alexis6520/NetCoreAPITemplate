using Domain.Entities;
using DomainServices.Common;
using DomainServices.DTOs.DemoItemDTOs;

namespace DomainServices.Repositories
{
    public interface IDemoItemRepository : IRepository<DemoItem>
    {
        Task<List<DemoItemSearchDTO>> SearchAsync(string text);
    }
}
