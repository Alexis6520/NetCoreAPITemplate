using Application.ROP;
using Domain.Services;
using UnitTests.Services;

namespace UnitTests.Abstractions
{
    /// <summary>
    /// Clase base para las pruebas unitarias.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class BaseTest<TService> : IDisposable
    {
        private ApplicationDbContext? _dbContext;

        protected ApplicationDbContext DbContext => _dbContext ??= new MemoryDbContext(typeof(TService).Name);

        protected bool ContainsError<T>(Result<T> result, Error error)
        {
            return result.Errors.Any(e => e.Code == error.Code);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
