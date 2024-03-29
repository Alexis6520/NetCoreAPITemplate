﻿using FluentValidation;
using Logic.Exceptions;
using MediatR;

namespace Logic.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null);

                if (failures.Any()) 
                {
                    var errors = failures.Select(x => new Failure(x.ErrorCode, x.ErrorMessage));
                    throw new Exceptions.ValidationException("Se han producido uno o más errores de validación.", errors);
                }

            }

            return await next();
        }
    }
}
