using ApplicationServices.Commands.DemoItemCommands;
using ApplicationServices.Handlers.DemoItemHandlers;
using ApplicationServices.Validators.DemoItemValidators;
using AutoMapper;
using Domain.Entities;
using DomainServices.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.DemoItemTests
{
    [TestClass]
    public class CreateDemoItemTest
    {
        private readonly Mock<IUnitOfWork> _contextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CreateDemoItemHandler>> _loggerMock = new();
        private readonly CreateDemoItemHandler _handler;

        public CreateDemoItemTest()
        {
            _handler = new(_contextMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        private static IEnumerable<object[]> ValidationTestData
        {
            get
            {
                return
                [
                    [new CreateDemoItemCommand()],
                    [new CreateDemoItemCommand() { Name = string.Empty }],
                    [new CreateDemoItemCommand() { Name = "  " }],
                    [new CreateDemoItemCommand() { Name = new string('A', 51) }],
                    [new CreateDemoItemCommand() { Name = "Cerveza", Description = new string('A', 601) }],
                    [new CreateDemoItemCommand() { Name = "Cerveza", Price = -1 }],
                ];
            }
        }

        [TestMethod]
        [DynamicData("ValidationTestData")]
        public async Task InputValidationAsync(CreateDemoItemCommand command)
        {
            var validator = new CreateDemoItemValidator();
            var result = await validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public async Task CreateDemoItemAsync()
        {
            var command = new CreateDemoItemCommand();
            var demoItem = new DemoItem();
            _mapperMock.Setup(x => x.Map<DemoItem>(command)).Returns(demoItem);
            _contextMock.Setup(x => x.DemoItems.AddAsync(demoItem, default)).Returns(Task.CompletedTask);
            await _handler.Handle(command, default);
        }
    }
}