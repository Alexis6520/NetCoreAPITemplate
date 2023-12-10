using System.Net;

namespace ApplicationServices.Exceptions
{
    public abstract class CustomException(string message, HttpStatusCode statusCode, IEnumerable<Error> errors) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
        public IEnumerable<Error> Errors { get; set; } = errors;
    }
}
