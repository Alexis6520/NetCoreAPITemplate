﻿using FluentValidation;
using Logic.Commands.DemoItemCommands;

namespace Logic.Validators.DemoItemValidators
{
    public class DemoItemUpdateValidator : AbstractValidator<DemoItemUpdateCommand>
    {
        public DemoItemUpdateValidator()
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
