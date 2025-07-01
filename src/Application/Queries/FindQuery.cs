using Application.ROP;
using MediatR;

namespace Application.Queries
{
    /// <summary>
    /// Consulta genérica para buscar un elemento por su clave.
    /// </summary>
    /// <typeparam name="TKey">Tipo de clave</typeparam>
    /// <typeparam name="TValue">Tipo de valor devuelto por la consulta</typeparam>
    /// <param name="key">Clave para buscar el elemento</param>
    public class FindQuery<TKey, TValue>(TKey key) : IRequest<Result<TValue>>
    {
        public TKey Key { get; } = key;
    }
}
