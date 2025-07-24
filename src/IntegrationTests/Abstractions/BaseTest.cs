using Domain.Services;
using IntegrationTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Abstractions
{
    public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
    {
        private IServiceScope? _scope;
        private AppDbContext? _dbContext;

        protected CustomWebAppFactory Factory => factory;

        protected AppDbContext DbContext
        {
            get
            {
                _scope ??= Factory.Services.CreateScope();

                _dbContext ??= _scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                return _dbContext;
            }
        }

        protected virtual void Cleanup() { }

        public void Dispose()
        {
            Cleanup();
            _dbContext?.Dispose();
            _dbContext = null;
            _scope?.Dispose();
            _scope = null;
            GC.SuppressFinalize(this);
        }
    }
}
