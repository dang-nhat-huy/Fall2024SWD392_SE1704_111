using System.Net;

namespace Application.ErrorHandlers
{
    public class ConflictException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.Conflict;
        const string title = "Resource Conflict";

        public ConflictException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public ConflictException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
