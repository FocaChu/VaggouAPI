using System.Net;

namespace VaggouAPI
{
    public class BusinessException : AppException
    {
        public BusinessException(string message)
            : base(message, HttpStatusCode.BadRequest) { }
    }
}
