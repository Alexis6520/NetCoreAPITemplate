using System.Collections.Immutable;
using System.Net;

namespace Application.ROP
{
    /// <summary>
    /// Representa el resultado de una operación que puede fallar por errores de dominio
    /// </summary>
    /// <typeparam name="T">Tipo de valor devuelto</typeparam>
    public readonly struct Result<T>
    {
        private Result(T? value, ImmutableArray<Error> errors)
        {
            Value = value;
            Errors = errors;
        }

        public T? Value { get; }
        public ImmutableArray<Error> Errors { get; }
        public readonly bool Succeeded => Errors.IsEmpty;

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, []);
        }

        public static Result<Unity> Success()
        {
            return Result<Unity>.Success(new());
        }

        public static Result<T> Failure(ImmutableArray<Error> errors)
        {
            if (errors.IsEmpty)
            {
                throw new ArgumentException("Tiene que haber al menos un error", nameof(errors));
            }

            return new Result<T>(default, errors);
        }

        public static Result<T> Failure(params Error[] errors)
        {
            return Failure(errors.ToImmutableArray());
        }
    }

    /// <summary>
    /// Representa un error de dominio  
    /// </summary>
    /// <param name="code">Código de error</param>
    /// <param name="Message">Descripción del error</param>
    public readonly struct Error(string code, string Message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        public string Code { get; } = code;
        public string Message { get; } = Message;
        public HttpStatusCode StatusCode { get; } = statusCode;
    }

    /// <summary>
    /// Estructura para operaciones que no devuelven un valor
    /// </summary>
    public struct Unity { }
}
