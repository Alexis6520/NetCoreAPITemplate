using Domain.Services;
using UnitTests.Services;

namespace UnitTests.Abstractions
{
    public abstract class BaseTest<TService> : IDisposable
    {
        private ApplicationDbContext? _dbContext;

        protected ApplicationDbContext DbContext => _dbContext ??= new MemoryDbContext(typeof(TService).Name);

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
