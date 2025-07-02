using Application.ROP;
using FluentValidation;
using System.Net;

namespace Application.Utils.Extensions
{
    public static class ValidatorExtension
    {
        public static async Task<Result<T>> ValidateAndMapAsync<T>(this IValidator<T> validator, T instance)
        {
            var validationResult = await validator.ValidateAsync(instance);

            if (validationResult.IsValid)
            {
                return Result<T>.Success(instance);
            }

            var errors = validationResult.Errors
                .Select(e => new Error(e.ErrorCode, e.ErrorMessage));

            return Result<T>.Failure(HttpStatusCode.BadRequest, [.. errors]);
        }
    }
}
