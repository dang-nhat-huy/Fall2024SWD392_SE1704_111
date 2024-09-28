using System.Net;

namespace Application.ErrorHandlers
{
    public class ForbiddenException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.Forbidden;
        const string title = "Forbidden";

        public ForbiddenException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public ForbiddenException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
