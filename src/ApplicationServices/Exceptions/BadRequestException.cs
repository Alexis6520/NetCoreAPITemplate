using System.Net;

namespace ApplicationServices.Exceptions
{
    public class BadRequestException(string message) : CustomException(message, HttpStatusCode.BadRequest)
    {
    }
}
