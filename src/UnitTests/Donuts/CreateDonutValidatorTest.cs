using Application.Commands.Donuts.Create;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutValidatorTest : BaseTest<CreateDonutValidator>
    {
        public static IEnumerable<object[]> InputData =>
        [
            [new CreateDonutCommand {
                Name=string.Empty
            }],
            [new CreateDonutCommand {
                Name=new string('a', 51)
            }],
            [new CreateDonutCommand {
                Name="Donita",
                Description=new string('a', 101)
            }],
            [new CreateDonutCommand {
                Name="Donita",
                Description=new string('a', 101),
                Price=-0.01m
            }]
        ];

        [TestMethod]
        [DynamicData(nameof(InputData))]
        public async Task ValidateInput(CreateDonutCommand command)
        {
            // Arrange
            var validator = new CreateDonutValidator();
            // Act
            var result = await validator.ValidateAsync(command);
            // Assert
            Assert.IsFalse(result.IsValid, "El comando debería ser inválido");
        }
    }
}
