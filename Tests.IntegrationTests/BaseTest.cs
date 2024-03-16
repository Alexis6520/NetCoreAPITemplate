using Infrastructure.IntegrationTests.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Net.Http.Headers;

namespace Infrastructure.IntegrationTests
{
    public abstract class BaseTest(CustomWebApplicationFactory<Program> factory, string url) : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected CustomWebApplicationFactory<Program> _factory = factory;
        protected string _url = url;
        private HttpClient _httpClient;
        private IServiceScope _serviceScope;
        private IUnitOfWork _unitOfWork;

        public HttpClient Client
        {
            get
            {
                _httpClient ??= _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication(opts => opts.DefaultAuthenticateScheme = "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", opts => { });
                    });
                }).CreateClient();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
                return _httpClient;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                _serviceScope ??= _factory.Services.CreateScope();
                _unitOfWork ??= _serviceScope.ServiceProvider.GetService<IUnitOfWork>();
                return _unitOfWork;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null;
            _url = null;
            _unitOfWork = null;
            _serviceScope?.Dispose();
            _serviceScope = null;
            GC.SuppressFinalize(this);
        }
    }
}
