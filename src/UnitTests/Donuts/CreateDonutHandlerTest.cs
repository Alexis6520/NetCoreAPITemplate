using Application.Commands.Donuts.Create;
using Application.ROP;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutHandlerTest : BaseTest<CreateDonutHandler>
    {
        private readonly Mock<ILogger<CreateDonutHandler>> _loggerMock = new();
        private readonly Mock<IValidator<CreateDonutCommand>> _validatorMock = new();

        private readonly CreateDonutCommand _command = new()
        {
            Name = "Frambuesa",
            Description = "Deliciosa donita de frambuesa",
            Price = 1.99m
        };

        [TestInitialize]
        public void Setup()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateDonutCommand>(), default))
                .ReturnsAsync(new ValidationResult());
        }

        private CreateDonutHandler CreateHandler()
        {
            return new CreateDonutHandler(DbContext, _loggerMock.Object, _validatorMock.Object);
        }

        [TestMethod]
        public async Task HappyPath()
        {
            CreateDonutHandler handler = CreateHandler();
            Result<int> result = await handler.Handle(_command, default);
            Assert.IsFalse(result.Succeeded, "La operación debería haber sido exitosa");
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        [TestMethod]
        public async Task DonutNameUnavailable()
        {
            // Arrange
            var command = new CreateDonutCommand
            {
                Name = "Chocolate",
            };

            DbContext.Donuts
                .Add(new Donut
                {
                    Name = command.Name,
                });

            await DbContext.SaveChangesAsync();
            CreateDonutHandler handler = CreateHandler();
            // Act
            Result<int> result = await handler.Handle(command, default);
            // Assert
            Assert.IsFalse(result.Succeeded, "La operación debería haber fallado");
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
            Assert.IsTrue(ContainsError(result, Errors.DONUT_NAME_NOT_AVAILABLE), "No se devolvió el error adecuado");
        }
    }
}
