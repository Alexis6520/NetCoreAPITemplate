namespace Services.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetAsync(object[] id, CancellationToken cancellationToken = default);
    }
}
