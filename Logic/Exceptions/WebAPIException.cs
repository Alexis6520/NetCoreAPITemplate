using System.Net;

namespace Logic.Exceptions
{
    public abstract class WebAPIException(HttpStatusCode statusCode, string message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
