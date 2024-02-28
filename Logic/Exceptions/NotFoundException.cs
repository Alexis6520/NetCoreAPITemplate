using System.Net;

namespace Logic.Exceptions
{
    public class NotFoundException(string message) : WebAPIException(HttpStatusCode.NotFound, message)
    {
    }
}
