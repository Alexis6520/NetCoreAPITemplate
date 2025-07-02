namespace IntegrationTests.Models
{
    public class Response<T>
    {
        public T? Value { get; set; }
        public IEnumerable<ResponseError>? Errors { get; set; }
    }

    public class ResponseError
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
