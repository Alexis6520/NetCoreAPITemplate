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
    public class DeleteDemoItemTest
    {
        private readonly DeleteDemoItemHandler _handler;
        private readonly Mock<IUnitOfWork> _contextMock;

        public DeleteDemoItemTest()
        {
            _contextMock = new Mock<IUnitOfWork>();
            _handler = new DeleteDemoItemHandler(_contextMock.Object);
        }

        private static IEnumerable<object[]> ValidationData
        {
            get
            {
                return [
                    [new DeleteDemoItemCommand { Id = 0 }],
                    [new DeleteDemoItemCommand { Id = -1 }],
                ];
            }
        }

        [TestMethod]
        [DynamicData("ValidationData")]
        public async Task ValidateWrongInputs(DeleteDemoItemCommand command)
        {
            var validator = new DeleteDemoItemValidator();
            var result = await validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public async Task SimpleDelete()
        {
            var command = new DeleteDemoItemCommand();
            var demoItem = new DemoItem();

            _contextMock.Setup(x => x.DemoItems.FindAsync(x => x.Id == command.Id, default))
                .ReturnsAsync(demoItem);

            await _handler.Handle(command, default);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task DeleteNonExistingItem()
        {
            var command = new DeleteDemoItemCommand();
            DemoItem demoItem = null;

            _contextMock.Setup(x => x.DemoItems.FindAsync(x => x.Id == command.Id, default))
                .ReturnsAsync(demoItem);

            await _handler.Handle(command, default);
        }
    }
}
