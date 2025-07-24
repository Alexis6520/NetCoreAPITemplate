using Application.Commands.Donuts;
using Domain.Entities;
using IntegrationTests.Abstractions;
using IntegrationTests.Models;
using IntegrationTests.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class DonutsTest(CustomWebAppFactory factory) : BaseTest(factory)
    {
        private const string BaseUrl = "api/donuts";
        private readonly List<int> _donutIds = [];

        [Fact]
        public async Task Create()
        {
            var command = new CreateDonutCommand
            {
                Name = "Frambuesa",
                Description = "La mejor dona del mundo",
                Price = 19.99m
            };

            var client = Factory.CreateClient();
            var response = await client.PostAsJsonAsync(BaseUrl, command);
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<Response<int>>();

            Assert.NotNull(result);

            bool exists = await DbContext.Donuts
                .AnyAsync(x => x.Id == result.Value);

            Assert.True(exists);
            _donutIds.Add(result.Value);
        }

        [Fact]
        public async Task GetList()
        {
            var donuts = new List<Donut>();

            for (int i = 0; i < 3; i++)
            {
                donuts.Add(new Donut
                {
                    Name = $"DonutList{i}"
                });
            }

            DbContext.Donuts.AddRange(donuts);
            await DbContext.SaveChangesAsync();
            _donutIds.AddRange(donuts.Select(x => x.Id));
            var client = Factory.CreateClient();
            var response = await client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<Response<List<Donut>>>();

            Assert.NotNull(result?.Value);
            Assert.True(result.Value.Count >= donuts.Count);
        }

        protected override void Cleanup()
        {
            if (_donutIds.Count > 0)
            {
                DbContext.Donuts
                    .Where(x => _donutIds.Contains(x.Id))
                    .ExecuteDelete();
            }
        }
    }
}