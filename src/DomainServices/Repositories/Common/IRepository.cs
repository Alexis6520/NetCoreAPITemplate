using System.Linq.Expressions;

namespace DomainServices.Repositories.Common
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
