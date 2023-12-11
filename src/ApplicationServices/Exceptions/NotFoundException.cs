using System.Net;

namespace ApplicationServices.Exceptions
{
    public class NotFoundException(string message) : CustomException(message, HttpStatusCode.NotFound)
    {
    }
}
