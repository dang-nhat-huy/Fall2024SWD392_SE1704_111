using System.Net;

namespace Application.ErrorHandlers
{
    public class UnauthorizedException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.Unauthorized;
        const string title = "Unauthorized";

        public UnauthorizedException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public UnauthorizedException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
