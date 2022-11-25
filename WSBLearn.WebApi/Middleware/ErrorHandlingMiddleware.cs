using WSBLearn.Application.Constants;
using WSBLearn.Application.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace WSBLearn.WebApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (ValidationException validationException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(validationException.Message);
            }
            catch (BadHttpRequestException badHttpRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badHttpRequestException.Message);
            }            
            catch (ArgumentException argumentException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(argumentException.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(Messages.GenericErrorMessage);
            }
        }
    }
}
