namespace API.Wrappers
{
    public class Response<T>(T data)
    {
        public T Data { get; set; } = data;
    }
}
