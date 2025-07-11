using Application.Commands.Donuts.Create;
using Application.ROP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutHandlerTest : BaseTest<CreateDonutHandler>
    {
        private readonly Mock<ILogger<CreateDonutHandler>> _logger = new();

        [TestMethod]
        public async Task HappyPath()
        {
            var command = new CreateDonutCommand
            {
                Name = "Frambuesa",
                Description = "La mejor dona del mundo",
                Price = 19.9m
            };

            var handler = new CreateDonutHandler(DbContext, _logger.Object);
            Result<int> result = await handler.Handle(command, default);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsTrue(await DbContext.Donuts.AnyAsync(x => x.Id == result.Value));
        }
    }
}
