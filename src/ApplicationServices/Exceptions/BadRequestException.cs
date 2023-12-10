using System.Net;

namespace ApplicationServices.Exceptions
{
    public class BadRequestException(string message, IEnumerable<Error> errors = null) : CustomException(message, HttpStatusCode.BadRequest, errors)
    {
    }
}
