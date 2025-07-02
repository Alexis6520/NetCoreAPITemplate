using Application.Commands.Donuts.Create;
using Application.Queries.Donuts.DTOs;
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
        private const string BaseUrl = "/api/donuts";
        private readonly List<int> _toDelete = [];

        [Fact]
        public async Task Create()
        {
            var command = new CreateDonutCommand
            {
                Name = "CreateTest",
                Description = "CreateTestDescription",
                Price = 10.99m,
            };

            HttpClient client = Factory.CreateClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(BaseUrl, command);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Response<int>>();
            Assert.NotNull(result);
            bool exists = await DbContext.Donuts.AnyAsync(x => x.Id == result.Value);
            Assert.True(exists, "La dona no se guardó en la base de datos");
            _toDelete.Add(result.Value);
        }

        [Fact]
        public async Task GetList()
        {
            var donuts = new List<Donut>();

            for (int i = 0; i < 5; i++)
            {
                var donut = new Donut
                {
                    Name = $"TestDonutList{i}",
                    Description = $"TestDescription",
                    Price = 1.99m
                };

                donuts.Add(donut);
            }

            DbContext.Donuts.AddRange(donuts);
            await DbContext.SaveChangesAsync();
            _toDelete.AddRange(donuts.Select(x => x.Id));
            HttpClient client = Factory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Response<List<DonutItemDTO>>>();
            Assert.NotNull(result?.Value);
            Assert.True(result.Value.Count >= donuts.Count);
        }

        protected override void Cleanup()
        {
            if (_toDelete.Count > 0)
            {
                DbContext.Donuts
                    .Where(x => _toDelete.Contains(x.Id))
                    .ExecuteDelete();
            }
        }
    }
}
