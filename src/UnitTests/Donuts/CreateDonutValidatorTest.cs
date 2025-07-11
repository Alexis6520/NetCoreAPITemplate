using Application.Commands.Donuts.Create;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutValidatorTest
    {
        public static IEnumerable<object[]> InputData =>
        [
            [new CreateDonutCommand {
                Name=string.Empty,
            }],
            [new CreateDonutCommand {
                Name=new string('A',51),
            }],
            [new CreateDonutCommand {
                Name="Frambuesa",
                Description=new string('A',101)
            }],
            [new CreateDonutCommand {
                Name="Frambuesa",
                Price=-0.01m
            }],
        ];

        private readonly CreateDonutValidator _validator = new CreateDonutValidator();

        [TestMethod]
        [DynamicData(nameof(InputData))]
        public async Task WrongInputs(CreateDonutCommand command)
        {
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
        }
    }
}
