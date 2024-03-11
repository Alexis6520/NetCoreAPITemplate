using Logic.Commands.DemoItemCommands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.DTOs.DemoItemDTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Infrastructure.IntegrationTests
{
    public class DemoItemModuleTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory = factory;
        private readonly string _url = "api/DemoItems";

        /// <summary>
        /// Crea un artículo demo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostDemoItemAsync()
        {
            using var client = GetDefaultClient();
            var command = new DemoItemCreateCommand("Cheetos", 15);
            var response = await client.PostAsJsonAsync(_url, command);
            response.EnsureSuccessStatusCode();
            var id = response.Content.ReadFromJsonAsync<int>();
            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var demoItem = unitOfWork.DemoItems.GetAsync([id]);
            Assert.NotNull(demoItem);
        }

        /// <summary>
        /// Intenta crear un artículo sin estar autenticado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnauthorizedPostDemoItemAsync()
        {
            using var client = _factory.CreateClient();
            var command = new DemoItemCreateCommand("Cheetos", 15);
            var response = await client.PostAsJsonAsync(_url, command);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        /// <summary>
        /// Obtiene los artículos demo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetDemoItemsAsync()
        {
            using var client = GetDefaultClient();
            var response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<List<DemoItemDTO>>();
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
        }

        /// <summary>
        /// Obtiene los artículos demo sin estar autenticado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnauthorizedGetDemoItemsAsync()
        {
            using var client = _factory.CreateClient();
            var response = await client.GetAsync(_url);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        /// <summary>
        /// Actualiza un artículo demo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutDemoItemAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var demoItem = (await unitOfWork.DemoItems.GetAllAsync()).First();
            var url = $"{_url}/{demoItem.Id}";
            var command = new DemoItemUpdateCommand("Picafresa", 1);
            using var client = GetDefaultClient();
            var response = await client.PutAsJsonAsync(url, command);
            response.EnsureSuccessStatusCode();
            var result = await unitOfWork.DemoItems.GetAsync([demoItem.Id]);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Price, result.Price);
        }

        /// <summary>
        /// Actualiza un artículo demo sin autenticar
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnauthorizedPutDemoItemAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var demoItem = (await unitOfWork.DemoItems.GetAllAsync()).First();
            var url = $"{_url}/{demoItem.Id}";
            var command = new DemoItemUpdateCommand("Picafresa", 1);
            using var client = _factory.CreateClient();
            var response = await client.PutAsJsonAsync(url, command);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        /// <summary>
        /// Elimina un artículo demo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteDemoItemAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var demoItem = (await unitOfWork.DemoItems.GetAllAsync()).First();
            var url = $"{_url}/{demoItem.Id}";
            using var client = GetDefaultClient();
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await unitOfWork.DemoItems.GetAsync([demoItem.Id]);
            Assert.Null(result);
        }

        /// <summary>
        /// Elimina un artículo demo sin autenticar
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnauthorizedDeleteDemoItemAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var demoItem = (await unitOfWork.DemoItems.GetAllAsync()).First();
            var url = $"{_url}/{demoItem.Id}";
            using var client = _factory.CreateClient();
            var response = await client.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private HttpClient GetDefaultClient()
        {
            // Configura la la petición para utilizar el esquema de autenticación de pruebas
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(opts => opts.DefaultAuthenticateScheme = "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", opts => { });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
            return client;
        }
    }
}
