using Application.Commands.Donuts;

namespace UnitTests.Donuts
{
    [TestClass]
    public sealed class CreateDonutValidatorTest
    {
        private readonly CreateDonutValidator _validator = new();

        public static IEnumerable<object[]> Inputs =>
        [
            [new CreateDonutCommand {
                Name=string.Empty
            }],
            [new CreateDonutCommand {
                Name=new string('A',51)
            }],
            [new CreateDonutCommand {
                Name="Frambuesa",
                Description=new string('A',101)
            }],
            [new CreateDonutCommand {
                Name="Frambuesa",
                Price=-0.01m
            }]
        ];

        [TestMethod]
        [DynamicData(nameof(Inputs))]
        public async Task WrongInputs(CreateDonutCommand command)
        {
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
        }
    }
}
