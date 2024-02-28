namespace API.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
