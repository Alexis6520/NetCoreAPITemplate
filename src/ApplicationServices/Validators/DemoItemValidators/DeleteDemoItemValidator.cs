using ApplicationServices.Commands.DemoItemCommands;
using FluentValidation;

namespace ApplicationServices.Validators.DemoItemValidators
{
    public class DeleteDemoItemValidator : AbstractValidator<DeleteDemoItemCommand>
    {
        public DeleteDemoItemValidator() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithErrorCode("InvalidFormat")
                .WithMessage("Formato de identificador inválido");
        }
    }
}
