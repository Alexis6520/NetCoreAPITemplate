using System.Collections.Immutable;
using System.Net;
using System.Text.Json.Serialization;

namespace Application.ROP
{
    /// <summary>
    /// Representa el resultado de una operación que puede fallar por errores de dominio
    /// </summary>
    /// <typeparam name="T">Tipo de valor devuelto</typeparam>
    public readonly struct Result<T>
    {
        private Result(T? value, HttpStatusCode statusCode)
        {
            Value = value;
            StatusCode = statusCode;
            Errors = [];
        }

        private Result(ImmutableArray<Error> errors, HttpStatusCode statusCode)
        {
            Value = default;
            StatusCode = statusCode;
            Errors = errors;
        }

        private static readonly Unity Unity = new();

        public T? Value { get; }
        public ImmutableArray<Error> Errors { get; }

        [JsonIgnore]
        public readonly bool Succeeded => Errors.IsEmpty;

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; }

        public static Result<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El código de estado debe ser menor a 400");
            }

            return new Result<T>(value, statusCode);
        }

        public static Result<Unity> Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
        {
            return Result<Unity>.Success(Unity, statusCode);
        }

        public static Result<T> Failure(ImmutableArray<Error> errors, HttpStatusCode statusCode)
        {
            if (errors.IsEmpty)
            {
                throw new ArgumentException("Tiene que haber al menos un error", nameof(errors));
            }

            if(statusCode < HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El código de estado debe ser mayor o igual a 400");
            }

            return new Result<T>(errors, statusCode);
        }

        public static Result<T> Failure(HttpStatusCode statusCode, params Error[] errors)
        {
            return Failure([.. errors], statusCode);
        }
    }

    /// <summary>
    /// Representa un error de dominio  
    /// </summary>
    /// <param name="code">Código de error</param>
    /// <param name="Message">Descripción del error</param>
    public readonly struct Error(string code, string Message)
    {
        public string Code { get; } = code;
        public string Message { get; } = Message;
    }

    /// <summary>
    /// Estructura para operaciones que no devuelven un valor
    /// </summary>
    public struct Unity { }
}
