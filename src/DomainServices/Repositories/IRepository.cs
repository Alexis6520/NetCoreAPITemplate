using System.Linq.Expressions;

namespace DomainServices.Common
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Marca la entidad para ser insertada al ejecutar SaveChangesAsync
        /// </summary>
        /// <param name="entity">Objeto a agregar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns></returns>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca un registro que cumple con la query
        /// </summary>
        /// <param name="predicate">Query con Linq</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Entidad que cumple con la query</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Devuelve todos los registros que cumplen con la query
        /// </summary>
        /// <param name="predicate">Query con Linq</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Entidades que cumplen con la query</returns>
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marca una entidad para su eliminación al ejecutar SaveChangesAsync
        /// </summary>
        /// <param name="entity">Objeto a eliminar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns></returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
