using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LearningApp.WebApi.Tests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new []
                {
                    new Claim(JwtRegisteredClaimNames.Jti, "1"),
                    new Claim(JwtRegisteredClaimNames.GivenName, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Email, "admin@mail.com"),
                    new Claim(ClaimTypes.Role, "Admin")
                }));

            context.HttpContext.User = claimsPrincipal;
            await next();
        }
    }
}
