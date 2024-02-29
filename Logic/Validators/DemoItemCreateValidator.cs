using FluentValidation;
using Logic.Commands;

namespace Logic.Validators
{
    public class DemoItemCreateValidator : AbstractValidator<DemoItemCreateCommand>
    {
        public DemoItemCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("{PropertyName} no puede estar vacio")
                .MaximumLength(50)
                .WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres");

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} no puede ser menor a {ComparisonValue}");
        }
    }
}
