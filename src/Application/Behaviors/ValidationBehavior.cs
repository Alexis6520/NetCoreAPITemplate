using Application.RP;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                Error[] errors = await Validate(request, cancellationToken);

                if (errors.Length > 0)
                {
                    var response = (TResponse?)Activator.CreateInstance(typeof(TResponse), HttpStatusCode.BadRequest, errors);

                    return response ?? throw new Exception("No se pudo crear la respuesta de validación fallida");
                }
            }

            return await next(cancellationToken);
        }

        private async Task<Error[]> Validate(TRequest request, CancellationToken cancellationToken)
        {
            var valContext = new ValidationContext<TRequest>(request);
            var valTasks = _validators.Select(x => x.ValidateAsync(valContext, cancellationToken));
            var results = await Task.WhenAll(valTasks);

            var errors = results.Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .Select(x => new Error(x.ErrorCode, x.ErrorMessage));

            return [.. errors];
        }
    }
}
