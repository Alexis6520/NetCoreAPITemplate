using FluentValidation;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutValidator : AbstractValidator<CreateDonutCommand>
    {
        public CreateDonutValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nombre requerido")
                .MaximumLength(50).WithMessage("El nombre no puede superar los {MaxLength} caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("La descripción no puede superar los {MaxLength} caracteres");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a {ComparisonValue}");
        }
    }
}
