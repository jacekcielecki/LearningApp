using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace LearningApp.Application.Tests.Helpers
{
    public class FakeHttpContextSingleton
    {
        private static ClaimsPrincipal? _claimsPrincipal;

        public static ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                if (_claimsPrincipal is not null) { return _claimsPrincipal; }

                _claimsPrincipal = new ClaimsPrincipal();
                _claimsPrincipal.AddIdentity(new ClaimsIdentity(
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, "999"),
                        new Claim(JwtRegisteredClaimNames.GivenName, "Admin"),
                        new Claim(JwtRegisteredClaimNames.Email, "admin@mail.com"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }));
                return _claimsPrincipal;
            }
        }
    }
}
