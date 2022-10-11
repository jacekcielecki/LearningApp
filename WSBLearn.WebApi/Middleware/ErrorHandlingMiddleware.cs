using FluentValidation;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Exceptions;

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
            catch (EntityContainsSubentityException entityContainsSubentityException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(entityContainsSubentityException.Message);
            }
            catch (ValidationException validationException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(validationException.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(Messages.GenericErrorMessage);
            }
        }
    }
}
