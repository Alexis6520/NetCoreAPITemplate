using Host.Features.Donuts.Commands;
using Host.Models;
using IntegrationTests.Abstractions;
using IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Net.Http.Json;

namespace IntegrationTests.Features.Donuts;

public class DonutsControllerTests(CustomWebAppFactory factory) : BaseTest(factory)
{
    private const string BaseUrl = "/api/donuts";

    [Fact]
    public async Task CreateShouldSaveInDatabase()
    {
        var donutId = await CreateDonut();

        bool exists = await DbContext.Donuts
            .AnyAsync(x => x.Id == donutId, CancellationToken);

        Assert.True(exists, "Donut not found in database");
    }

    private async Task<int> CreateDonut()
    {
        var request = new CreateDonutRequest(
            "Frambuesa",
            "La mejor dona de la tienda",
            19.99m
        );

        using var res = await Client.PostAsJsonAsync(BaseUrl, request, CancellationToken);
        res.EnsureSuccessStatusCode();

        var content = await res.Content
            .ReadFromJsonAsync<Response<int>>(CancellationToken);

        Assert.NotNull(content);
        return content.Value;
    }

    [Fact]
    public async Task GetAllShouldRetrieveAllDonuts()
    {
        int n = 3;

        for (int i = 0; i < n; i++)
        {
            await CreateDonut();
        }

        using var res = await Client.GetAsync(BaseUrl, CancellationToken);
        res.EnsureSuccessStatusCode();

        var content = await res.Content
            .ReadFromJsonAsync<Response<Donut[]>>(CancellationToken);

        Assert.NotNull(content?.Value);
        Assert.True(content.Value.Length >= n, $"Expected at least {n} donuts");
    }
}
