using IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Abstractions;

public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope = factory.Services.CreateScope();
    private ApplicationDbContext? _dbContext;

    protected CustomWebAppFactory Factory { get; } = factory;
    protected HttpClient Client => Factory.CreateClient();
    protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    protected ApplicationDbContext DbContext =>
        _dbContext ??= _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    public void Dispose()
    {
        _dbContext?.Dispose();
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
