﻿using Domain.Services;
using IntegrationTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Abstractions
{
    public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>,IDisposable
    {
        protected CustomWebAppFactory Factory => factory;
        private IServiceScope? _scope;
        private ApplicationDbContext? _dbContext;

        protected ApplicationDbContext DbContext
        {
            get
            {
                _scope ??= Factory.Services.CreateScope();

                _dbContext ??= _scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                return _dbContext;
            }
        }

        protected virtual void Cleanup() { }

        public void Dispose()
        {
            Cleanup();
            _scope?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
