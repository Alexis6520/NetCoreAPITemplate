namespace Host.Models;

public class Response<T>(T value)
{
    public T Value { get; set; } = value;
}
