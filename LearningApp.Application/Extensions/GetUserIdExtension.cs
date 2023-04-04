using Microsoft.AspNetCore.Http;

namespace LearningApp.Application.Extensions
{
    public static class GetUserIdExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            //if (httpContext.User is null)
            //    return string.Empty;

            return int.Parse(httpContext.User.Claims.Single(c => c.Type == "jti").Value);
        }
    }
}
