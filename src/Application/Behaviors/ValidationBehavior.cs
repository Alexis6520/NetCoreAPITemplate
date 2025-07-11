using Application.ROP;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;
using System.Reflection;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var responseType = typeof(TResponse);

            if (
                !responseType.IsGenericType ||
                responseType.GetGenericTypeDefinition() != typeof(Result<>))
            {
                return await next(cancellationToken);
            }

            if (_validators.Any())
            {
                Error[] errors = await ValidateAsync(request, cancellationToken);

                if (errors.Length > 0)
                {
                    return CreateResult(errors, responseType);
                }
            }

            return await next(cancellationToken);
        }

        private async Task<Error[]> ValidateAsync(TRequest request, CancellationToken cancellationToken)
        {
            var valContext = new ValidationContext<TRequest>(request);

            IEnumerable<Task<ValidationResult>> valTasks =
                _validators.Select(x => x.ValidateAsync(valContext));

            ValidationResult[] results = await Task.WhenAll(valTasks);

            IEnumerable<Error> errors = results.Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .Select(x => new Error(x.ErrorCode, x.ErrorMessage));

            return [.. errors];
        }

        private static TResponse CreateResult(Error[] errors, Type responseType)
        {
            var innerType = responseType.GetGenericArguments()[0];

            var failMethod = typeof(Result<>)
                .MakeGenericType(innerType)
                .GetMethod("Failure", BindingFlags.Public | BindingFlags.Static, [typeof(HttpStatusCode), typeof(Error[])]);

            _ = failMethod ?? throw new Exception("No se encontró el método Failure");
            var result = failMethod.Invoke(null, [HttpStatusCode.BadRequest, errors]);
            return (TResponse)result!;
        }
    }
}
