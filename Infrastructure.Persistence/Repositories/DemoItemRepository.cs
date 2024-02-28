using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class DemoItemRepository(ApplicationDbContext dbContext, IMapper mapper) : IDemoItemRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task AddAsync(DemoItem entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        public async Task<List<DemoItemDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.DemoItems
                .ProjectTo<DemoItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<DemoItem> GetAsync(object[] id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.DemoItems.FindAsync(id, cancellationToken);
        }

        public void Remove(DemoItem entity)
        {
            _dbContext.DemoItems.Remove(entity);
        }
    }
}
