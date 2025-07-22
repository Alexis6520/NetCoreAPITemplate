using System.Net;
using System.Text.Json.Serialization;

namespace Application.RP
{
    public class Error(string code, string message)
    {
        public string Code { get; } = code;
        public string Message { get; } = message;
    }

    public class Result
    {
        public Result(HttpStatusCode statusCode = HttpStatusCode.NoContent)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El código no representa un estado exitoso");
            }

            StatusCode = statusCode;
            Errors = [];
        }

        public Result(HttpStatusCode statusCode, params Error[] errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statusCode),
                    statusCode,
                    "El código no representa un estado de error");
            }

            if (errors.Length == 0)
            {
                throw new Exception("Debe ingresarse al menos un error");
            }

            StatusCode = statusCode;
            Errors = errors;
        }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class Result<T> : Result
    {
        public Result(T value, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode)
        {
            Value = value;
        }

        public Result(HttpStatusCode statusCode, params Error[] errors) : base(statusCode, errors) { }

        public T? Value { get; set; }
    }
}
