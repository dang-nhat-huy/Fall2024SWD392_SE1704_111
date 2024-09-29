using System.Net;

namespace Application.ErrorHandlers
{
    public class BadRequestException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.BadRequest;
        const string title = "Bad Request";

        public BadRequestException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public BadRequestException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
