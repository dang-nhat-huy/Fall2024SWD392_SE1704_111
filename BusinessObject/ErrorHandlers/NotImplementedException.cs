using System.Net;

namespace Application.ErrorHandlers
{
    public class NotImplementedException : BaseException
    {
        const int statusCode = (int)HttpStatusCode.NotImplemented;
        const string title = "Requested Function Not Implemented";

        public NotImplementedException()
        {
            StatusCode = statusCode;
            Title = title;
        }

        public NotImplementedException(string? message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
