using Application.Commands.Donuts.Create;
using Application.Queries.Donuts.DTOs;
using Domain.Entities;
using Host.Abstractions;
using IntegrationTests.Abstractions;
using IntegrationTests.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class Donuts(CustomWebAppFactory factory) : BaseTest(factory)
    {
        private const string BaseUrl = "api/donuts";
        private readonly List<int> _donuts = [];

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
            Assert.True(await DbContext.Donuts.AnyAsync(x => x.Id == result.Value));
            _donuts.Add(result.Value);
        }

        [Fact]
        public async Task GetList()
        {
            var donuts = new List<Donut>();

            for (int i = 0; i < 3; i++)
            {
                donuts.Add(new Donut
                {
                    Name = $"Lista-{i}"
                });
            }

            DbContext.Donuts.AddRange(donuts);
            await DbContext.SaveChangesAsync();
            _donuts.AddRange(donuts.Select(x => x.Id));
            var client = Factory.CreateClient();
            var response = await client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<Response<List<DonutDTO>>>();

            Assert.NotNull(result?.Value);
            Assert.True(result.Value.Count >= donuts.Count);
        }

        protected override void Cleanup()
        {
            if (_donuts.Count > 0)
            {
                DbContext.Donuts
                    .Where(x => _donuts.Contains(x.Id))
                    .ExecuteDelete();
            }
        }
    }
}