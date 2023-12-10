using ApplicationServices.Commands.DemoItemCommands;
using FluentValidation;

namespace ApplicationServices.Validators
{
    public class CreateDemoItemValidator : AbstractValidator<CreateDemoItemCommand>
    {
        public CreateDemoItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode("NameEmpty").WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithErrorCode("LengthExceeded").WithMessage("El nombre no puede superar los {MaxLength} caracteres.");

            RuleFor(x=> x.Description)
                .MaximumLength(600).WithErrorCode("LengthExceeded").WithMessage("La descripción no puede superar los {MaxLength} caracteres.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithErrorCode("InvalidQuantity").WithMessage("El precio debe ser mayor o igual a {ComparisonValue}");
        }
    }
}
