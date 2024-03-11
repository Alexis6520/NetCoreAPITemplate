using FluentValidation;
using Logic.Commands.DemoItemCommands;

namespace Logic.Validators.DemoItemValidators
{
    public class DemoItemDeleteValidator : AbstractValidator<DemoItemDeleteCommand>
    {
        public DemoItemDeleteValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El identificador debe ser mayor a {ComparisonValue}");
        }
    }
}
