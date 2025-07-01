using System.Net;

namespace VaggouAPI
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, HttpStatusCode.NotFound) { }
    }
}
