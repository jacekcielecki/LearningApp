using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LearningApp.Application.Extensions
{
    public static class GetUserContextExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.Claims.SingleOrDefault(c => c.Type == "jti");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User ID claim not found or invalid.");
        }

        public static ClaimsPrincipal GetUserContext(this HttpContext httpContext) => httpContext?.User;

        public static int GetUserId(this ClaimsPrincipal userClaim)
        {
            var userIdClaim = userClaim.Claims.SingleOrDefault(c => c.Type == "jti");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User ID claim not found or invalid.");
        }
    }
}
