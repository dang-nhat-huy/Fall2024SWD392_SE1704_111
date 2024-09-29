using System.Net;

namespace Application.ErrorHandlers
{
    public class NotFoundException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.NotFound;
        const string title = "Resource Not Found";

        public NotFoundException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public NotFoundException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
