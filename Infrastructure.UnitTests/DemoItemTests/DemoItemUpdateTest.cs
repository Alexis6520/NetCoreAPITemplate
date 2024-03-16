using Domain;
using Logic.Commands.DemoItemCommands;
using Logic.Exceptions;
using Logic.Handlers.DemoItemHandlers;
using Logic.Validators.DemoItemValidators;

namespace Infrastructure.UnitTests.DemoItemTests
{
    [TestClass]
    public class DemoItemUpdateTest : BaseTest<DemoItemUpdateHandler>
    {
        public static IEnumerable<object[]> ValidationData
        {
            get
            {
                return new[]
                {
                    [new DemoItemUpdateCommand(string.Empty,100)],
                    [new DemoItemUpdateCommand(null,100)],
                    [new DemoItemUpdateCommand("  ",100)],
                    [new DemoItemUpdateCommand("Papas",-1)],
                    new[] {new DemoItemUpdateCommand(null,-1)},
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(DemoItemUpdateCommand command)
        {
            var validator = new DemoItemUpdateValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Actualización  simple de un artículo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task UpdateDemoItemAsync()
        {
            var demoItem = new DemoItem("Ruffles", 30) { Id = 1 };
            var param = new object[] { demoItem.Id };

            _demoItemRepoMock.Setup(x => x.GetAsync(param, CancellationToken.None))
                .Returns(Task.FromResult(demoItem));

            _unitOfWorkMock.Setup(x => x.DemoItems)
                .Returns(_demoItemRepoMock.Object);

            var changesSaved = false;
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).Callback(() => changesSaved = true);
            var command = new DemoItemUpdateCommand("Cheetos", 40) { Id = 1 };
            var handler = new DemoItemUpdateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            await handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(demoItem.Name == command.Name);
            Assert.IsTrue(demoItem.Price == command.Price);
            Assert.IsTrue(changesSaved);
        }

        /// <summary>
        /// Intenta actualizar un artículo que ya no existe
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TryUpdateNotExistingItemAsync()
        {
            var demoItem = new DemoItem("Ruffles", 30) { Id = 1 };
            var param = new object[] { demoItem.Id };

            _demoItemRepoMock.Setup(x => x.GetAsync(param, CancellationToken.None))
                .Returns(Task.FromResult<DemoItem>(null));

            _unitOfWorkMock.Setup(x => x.DemoItems)
                .Returns(_demoItemRepoMock.Object);

            var command = new DemoItemUpdateCommand("Cheetos", 40) { Id = 1 };
            var handler = new DemoItemUpdateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            await handler.Handle(command, CancellationToken.None);
        }
    }
}
