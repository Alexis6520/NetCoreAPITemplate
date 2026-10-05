using Host.Features.Donuts.Commands;
using UnitTests.Abstractions;

namespace UnitTests.Features.Donuts;

[TestClass]
public class CreateDonutHandlerTests : BaseTest
{
    [TestMethod]
    public async Task CreateShouldSucceed()
    {
        var request = new CreateDonutRequest(
            "Frambuesa",
            "La mejor dona de la tienda",
            19.99m
        );

        var handler = new CreateDonutHandler(DbContext);
        await handler.Execute(request, TestContext.CancellationToken);
    }
}
