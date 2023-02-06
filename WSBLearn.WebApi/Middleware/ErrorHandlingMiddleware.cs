using Newtonsoft.Json;
using System.Net;
using WSBLearn.Application.Exceptions;

namespace WSBLearn.WebApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                NotFoundException _ => HttpStatusCode.NotFound,
                UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            var result = JsonConvert.SerializeObject(new {error = exception.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}
