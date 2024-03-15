using Infrastructure.IntegrationTests.Services;
using Logic.Commands.DemoItemCommands;
using Services.DTOs.DemoItemDTOs;
using System.Net;
using System.Net.Http.Json;

namespace Infrastructure.IntegrationTests
{
    public class DemoItemModuleTests(CustomWebApplicationFactory<Program> factory) : BaseTest(factory, "api/DemoItems")
    {
        /// <summary>
        /// Crea un artículo demo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostDemoItemAsync()
        {
            var id = await CreateDemoItemAsync();
            var demoItem = UnitOfWork.DemoItems.GetAsync([id]);
            Assert.NotNull(demoItem);
        }

        private async Task<int> CreateDemoItemAsync()
        {
            var command = new DemoItemCreateCommand("Cheetos", 15);
            var response = await Client.PostAsJsonAsync(_url, command);
            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadFromJsonAsync<int>();
            return id;
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
            await CreateDemoItemAsync();
            var response = await Client.GetAsync(_url);
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
            var id = await CreateDemoItemAsync();
            var url = $"{_url}/{id}";
            var command = new DemoItemUpdateCommand("Picafresa", 1);
            var response = await Client.PutAsJsonAsync(url, command);
            response.EnsureSuccessStatusCode();
            var demoItem = await UnitOfWork.DemoItems.GetAsync([id]);
            var result = await UnitOfWork.DemoItems.GetAsync([demoItem.Id]);
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
            var url = $"{_url}/1";
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
            var id = await CreateDemoItemAsync();
            var url = $"{_url}/{id}";
            var response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await UnitOfWork.DemoItems.GetAsync([id]);
            Assert.Null(result);
        }

        /// <summary>
        /// Elimina un artículo demo sin autenticar
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnauthorizedDeleteDemoItemAsync()
        {
            var url = $"{_url}/1";
            using var client = _factory.CreateClient();
            var response = await client.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
