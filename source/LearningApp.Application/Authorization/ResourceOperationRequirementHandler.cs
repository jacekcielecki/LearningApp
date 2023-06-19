using LearningApp.Domain.Entities;
using LearningApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, UserContent>
    {
        private readonly LearningAppDbContext _dbContext;

        public ResourceOperationRequirementHandler(LearningAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, UserContent resource)
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
            switch (resource)
            {
                case Category:
                {
                    var isResourceCreator = userId is not null && resource.CreatorId == int.Parse(userId);
                    if (isResourceCreator) context.Succeed(requirement);
                    break;
                }
                case Question:
                {
                    var resourceCategory = _dbContext
                        .Categories
                        .FirstOrDefault(x => x.CreatorId == resource.CreatorId);

                    if (resourceCategory is not null)
                    {
                        var isResourceCategoryCreator = userId is not null && resourceCategory.CreatorId == int.Parse(userId);
                        if (isResourceCategoryCreator) context.Succeed(requirement);
                    }
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
