namespace API.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class Error(string code, string message)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
    }
}
