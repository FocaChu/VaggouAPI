using System.Net;

namespace VaggouAPI
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Acesso não autorizado.")
            : base(message, HttpStatusCode.Unauthorized) { }
    }
}
