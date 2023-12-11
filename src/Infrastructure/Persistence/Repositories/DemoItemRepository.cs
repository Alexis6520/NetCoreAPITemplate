using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using DomainServices.DTOs.DemoItemDTOs;
using DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class DemoItemRepository(ApplicationDbContext dbContext, IMapper mapper) : IDemoItemRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task AddAsync(DemoItem entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.DemoItems.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(DemoItem entity, CancellationToken cancellationToken = default)
        {
            _dbContext.DemoItems.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<DemoItem>> FindAllAsync(Expression<Func<DemoItem, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var results = await _dbContext.DemoItems
                .Where(predicate)
                .ToListAsync(cancellationToken);

            return results;
        }

        public async Task<DemoItem> FindAsync(Expression<Func<DemoItem, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.DemoItems
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<List<DemoItemSearchDTO>> SearchAsync(string text)
        {
            var results = await _dbContext.DemoItems
                .Where(x => EF.Functions.Like(x.Name, $"{text}%"))
                .ProjectTo<DemoItemSearchDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return results;
        }
    }
}