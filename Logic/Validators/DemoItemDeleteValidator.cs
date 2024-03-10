using FluentValidation;
using Logic.Commands;

namespace Logic.Validators
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
