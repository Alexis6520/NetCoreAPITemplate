namespace API.Responses
{
    public class ErrorResponse(string message = null, IEnumerable<Error> errors = null)
    {
        public string Message { get; set; } = message;
        public IEnumerable<Error> Errors { get; set; } = errors;
    }

    public class Error(string code, string message)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
    }
}
