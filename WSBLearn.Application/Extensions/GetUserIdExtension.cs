using Microsoft.AspNetCore.Http;

namespace WSBLearn.Application.Extensions
{
    public static class GetUserIdExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            //if (httpContext.User is null)
            //    return string.Empty;

            return Int32.Parse(httpContext.User.Claims.Single(c => c.Type == "jti").Value);
        }
    }
}
