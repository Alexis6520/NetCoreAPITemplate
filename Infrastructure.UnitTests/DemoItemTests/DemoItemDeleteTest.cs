using Logic.Exceptions;
using Moq;
using Services.Repositories;
using Services;
using Microsoft.Extensions.Logging;
using Logic.Commands.DemoItemCommands;
using Logic.Handlers.DemoItemHandlers;
using Logic.Validators.DemoItemValidators;
using Domain;

namespace Infrastructure.UnitTests.DemoItemTests
{
    [TestClass]
    public class DemoItemDeleteTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<DemoItemDeleteHandler>> _loggerMock = new();
        private readonly Mock<IDemoItemRepository> _demoItemRepoMock = new();

        public static IEnumerable<object[]> ValidationData
        {
            get
            {
                return new[]
                {
                    [new DemoItemDeleteCommand { Id=0}],
                    new[] { new DemoItemDeleteCommand { Id=-1} },
                };
            }
        }

        /// <summary>
        /// Coumprueba el validador del comando
        /// </summary>
        /// <param name="command"></param>
        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(DemoItemDeleteCommand command)
        {
            var validator = new DemoItemDeleteValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Actualización  simple de un artículo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeleteDemoItemAsync()
        {
            var demoItem = new DemoItem("Ruffles", 30) { Id = 1 };
            var param = new object[] { demoItem.Id };

            _demoItemRepoMock.Setup(x => x.GetAsync(param, CancellationToken.None))
                .Returns(Task.FromResult(demoItem));

            var wasRemoved = false;
            var changesSaved = false;

            _demoItemRepoMock.Setup(x => x.Remove(demoItem))
                .Callback(() => wasRemoved = true);

            _unitOfWorkMock.Setup(x => x.DemoItems)
                .Returns(_demoItemRepoMock.Object);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(CancellationToken.None))
                .Callback(() => changesSaved = wasRemoved);

            var command = new DemoItemDeleteCommand { Id = 1 };
            var handler = new DemoItemDeleteHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            await handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(wasRemoved);
            Assert.IsTrue(changesSaved);
        }

        /// <summary>
        /// Intenta eliminar un artículo que ya no existe
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TryDeleteNotExistingItemAsync()
        {
            var demoItem = new DemoItem("Ruffles", 30) { Id = 1 };
            var param = new object[] { demoItem.Id };

            _demoItemRepoMock.Setup(x => x.GetAsync(param, CancellationToken.None))
                .Returns(Task.FromResult<DemoItem>(null));

            _unitOfWorkMock.Setup(x => x.DemoItems)
                .Returns(_demoItemRepoMock.Object);

            var command = new DemoItemDeleteCommand() { Id = 1 };
            var handler = new DemoItemDeleteHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            await handler.Handle(command, CancellationToken.None);
        }
    }
}
