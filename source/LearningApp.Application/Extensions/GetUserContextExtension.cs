using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LearningApp.Application.Extensions
{
    public static class GetUserContextExtension
    {
        public static ClaimsPrincipal GetUserContext(this HttpContext httpContext) => httpContext?.User;

        public static int GetUserId(this ClaimsPrincipal userClaim)
        {
            var userIdClaim = userClaim.Claims.SingleOrDefault(c => c.Type == "jti");
            return Convert.ToInt32(userIdClaim?.Value);
        }

        public static string GetUserRole(this ClaimsPrincipal userClaim)
        {
            var userRoleClaim = userClaim.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);
            return userRoleClaim?.Value;
        }
    }
}
