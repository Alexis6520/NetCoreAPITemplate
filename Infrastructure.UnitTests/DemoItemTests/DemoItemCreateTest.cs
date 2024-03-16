using Logic.Commands.DemoItemCommands;
using Logic.Handlers.DemoItemHandlers;
using Logic.Validators.DemoItemValidators;
using Moq;
using Services.Repositories;

namespace Infrastructure.UnitTests.DemoItemTests
{
    [TestClass]
    public class DemoItemCreateTest : BaseTest<DemoItemCreateHandler>
    {
        public static IEnumerable<object[]> ValidationData
        {
            get
            {
                return new[]
                {
                    [new DemoItemCreateCommand(string.Empty,100)],
                    [new DemoItemCreateCommand(null,100)],
                    [new DemoItemCreateCommand("  ",100)],
                    [new DemoItemCreateCommand("Papas",-1)],
                    new[] {new DemoItemCreateCommand(null,-1)},
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(DemoItemCreateCommand command)
        {
            var validator = new DemoItemCreateValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Operación simple de creación de artículo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateDemoItemAsync()
        {
            _unitOfWorkMock.Setup(x => x.DemoItems).Returns(new Mock<IDemoItemRepository>().Object);
            var saved = false;
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).Callback(() => { saved = true; });
            var handler = new DemoItemCreateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var command = new DemoItemCreateCommand("Doritos", 50);
            await handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(saved);
        }
    }
}