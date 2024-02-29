namespace Logic.Exceptions
{
    public class ValidationException(string message, IEnumerable<Failure> failures) : Exception(message)
    {
        public IEnumerable<Failure> Failures { get; set; } = failures;
    }

    public class Failure(string code, string message)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
    }
}
