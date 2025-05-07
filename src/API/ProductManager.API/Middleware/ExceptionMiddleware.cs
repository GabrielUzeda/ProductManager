using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ProductManager.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro não tratado");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var notFoundResponse = new
                {
                    status = context.Response.StatusCode,
                    message = exception.Message,
                    details = "O recurso solicitado não foi encontrado."
                };
                
                var notFoundResult = JsonSerializer.Serialize(notFoundResponse);
                await context.Response.WriteAsync(notFoundResult);
                return;
            }
            
            // Para outros tipos de exceção, retornar erro 500
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new
            {
                status = context.Response.StatusCode,
                message = "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.",
                details = exception.Message
            };
            
            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }
}
