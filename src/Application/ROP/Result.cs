using System.Collections.Immutable;
using System.Net;

namespace Application.ROP
{
    public readonly struct Result<T>
    {
        private Result(T? value, ImmutableArray<Error> errors, HttpStatusCode statusCode)
        {
            Value = value;
            Errors = errors;
            StatusCode = statusCode;
        }

        public T? Value { get; }
        public ImmutableArray<Error> Errors { get; }
        public readonly bool Succeeded => Errors.Length == 0;
        public HttpStatusCode StatusCode { get; }

        public static Result<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El valor proporcionado no se encuentra en el rango de éxito");
            }

            return new(value, [], statusCode);
        }

        public static Result<Unity> Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
        {
            return Result<Unity>.Success(Unity.Value, statusCode);
        }

        public static Result<T> Failure(HttpStatusCode statusCode, ImmutableArray<Error> errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El valor proporcionado no se encuentra en el rango de error");
            }

            return new(default, errors, statusCode);
        }

        public static Result<T> Failure(HttpStatusCode statusCode, params Error[] errors)
        {
            return Result<T>.Failure(statusCode, errors.ToImmutableArray());
        }
    }

    public readonly struct Error(string code, string message)
    {
        public string Code { get; } = code;
        public string Message { get; } = message;
    }

    public readonly struct Unity
    {
        public static readonly Unity Value = new();
    }
}
