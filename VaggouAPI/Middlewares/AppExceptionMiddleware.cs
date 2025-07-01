using System.Net;
using System.Text.Json;
using FluentValidation;

namespace VaggouAPI.Middleware
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionMiddleware> _logger;

        public AppExceptionMiddleware(RequestDelegate next, ILogger<AppExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            object errorResponse;

            if (exception is AppException appEx)
            {
                statusCode = appEx.StatusCode;
                errorResponse = new { message = appEx.Message };
            }
            else if (exception is ValidationException validationEx)
            {
                statusCode = HttpStatusCode.BadRequest;

                var errors = validationEx.Errors?.Any() == true
                    ? validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    : new[] { new { PropertyName = "N/A", ErrorMessage = validationEx.Message } };

                errorResponse = new
                {
                    message = "Erros de validação",
                    errors
                };
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorResponse = new { message = "Erro interno no servidor." };
            }

            LogException(exception, statusCode);

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private void LogException(Exception ex, HttpStatusCode statusCode)
        {
            if ((int)statusCode >= 500)
                _logger.LogError(ex, "Erro interno não tratado");
            else
                _logger.LogWarning(ex, "Exceção tratada");
        }
    }
}
