using LearningApp.Domain.Entities;
using LearningApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LearningApp.Application.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Category>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Category resource)
        {
            if (requirement.ResourceOperation == OperationType.Read)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var isAdmin = userRole?.ToLower() == "admin";
            if (isAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var isResourceCreator = userId is not null && resource.CreatorId == int.Parse(userId);
            if (isResourceCreator)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
