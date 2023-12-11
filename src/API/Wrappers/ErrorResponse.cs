using ApplicationServices.Exceptions;

namespace API.Wrappers
{
    public class ErrorResponse(string message, IEnumerable<Error> errors = null)
    {
        public string Message { get; set; } = message;
        public IEnumerable<Error> Errors { get; set; } = errors;
    }
}
