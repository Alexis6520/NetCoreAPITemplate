using IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Abstractions;

public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
{
    private IServiceScope? _scope;
    private ApplicationDbContext? _dbContext;

    protected CustomWebAppFactory Factory { get; } = factory;
    protected HttpClient Client => Factory.CreateClient();
    protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    protected ApplicationDbContext DbContext =>
        _dbContext ??= GetService<ApplicationDbContext>();

    public void Dispose()
    {
        _dbContext?.Dispose();
        _scope?.Dispose();
        GC.SuppressFinalize(this);
    }

    protected T GetService<T>() where T : notnull
    {
        _scope ??= Factory.Services.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<T>();
    }
}
