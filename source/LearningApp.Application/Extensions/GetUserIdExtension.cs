using Microsoft.AspNetCore.Http;

namespace LearningApp.Application.Extensions
{
    public static class GetUserIdExtension
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
    }
}
