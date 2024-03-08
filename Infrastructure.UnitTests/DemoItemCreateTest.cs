using Logic.Commands;
using Logic.Validators;

namespace Infrastructure.UnitTests
{
    [TestClass]
    public class DemoItemCreateTest
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
    }
}