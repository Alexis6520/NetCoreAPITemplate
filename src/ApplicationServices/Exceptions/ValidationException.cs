using System.Net;

namespace ApplicationServices.Exceptions
{
    public class ValidationException(string message, IEnumerable<Error> errors) : CustomException(message, HttpStatusCode.BadRequest)
    {
        public IEnumerable<Error> Errors { get; } = errors;
    }
}
