using ApplicationServices.Commands.DemoItemCommands;
using ApplicationServices.Exceptions;
using ApplicationServices.Handlers.DemoItemHandlers;
using ApplicationServices.Validators.DemoItemValidators;
using Domain.Entities;
using DomainServices.Services;
using Moq;

namespace UnitTests.DemoItemTests
{
    [TestClass]
    public class UpdateDemoItemTests
    {
        private readonly Mock<IUnitOfWork> _contextMock = new();

        private static IEnumerable<object[]> ValidationTestData
        {
            get
            {
                return
                [
                    [new UpdateDemoItemCommand()],
                    [new UpdateDemoItemCommand() { Name = string.Empty }],
                    [new UpdateDemoItemCommand() { Name = "  " }],
                    [new UpdateDemoItemCommand() { Name = new string('A', 51) }],
                    [new UpdateDemoItemCommand() { Name = "Cerveza", Description = new string('A', 601) }],
                    [new UpdateDemoItemCommand() { Name = "Cerveza", Price = -1 }],
                ];
            }
        }

        [TestMethod]
        [DynamicData("ValidationTestData")]
        public async Task ValidateWrongInputs(UpdateDemoItemCommand command)
        {
            var validator = new UpdateDemoItemValidator();
            var result = await validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public async Task SimpleUpdate()
        {
            var demoItem = new DemoItem()
            {
                Name = "Tonayan",
                Description = "Elixir",
                Price = 1000
            };

            var command = new UpdateDemoItemCommand()
            {
                Name = "Vino tinto",
                Description = "Una basura",
                Price = 10
            };

            _contextMock.Setup(x => x.DemoItems.FindAsync(x => x.Id == command.Id, default))
                .ReturnsAsync(demoItem);

            var handler = new UpdateDemoItemHandler(_contextMock.Object);
            await handler.Handle(command, default);
            Assert.IsTrue(EntityUpdated(demoItem, command));
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdatenonexistingItem()
        {
            DemoItem demoItem = null;

            var command = new UpdateDemoItemCommand()
            {
                Name = "Vino tinto",
                Description = "Una basura",
                Price = 10
            };

            _contextMock.Setup(x => x.DemoItems.FindAsync(x => x.Id == command.Id, default))
                .ReturnsAsync(demoItem);

            var handler = new UpdateDemoItemHandler(_contextMock.Object);
            await handler.Handle(command, default);
        }

        private static bool EntityUpdated(DemoItem entity, UpdateDemoItemCommand command)
        {
            return entity.Name.Equals(command.Name) &&
                entity.Description.Equals(command.Description) &&
                entity.Price == command.Price;
        }
    }
}
