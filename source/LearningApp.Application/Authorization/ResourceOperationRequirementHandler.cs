using LearningApp.Application.Extensions;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Enums;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LearningApp.Application.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, UserContent>
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResourceOperationRequirementHandler(LearningAppDbContext dbContext, 
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement, 
            UserContent resource)
        {
            ClaimsPrincipal userContext = _httpContextAccessor.HttpContext?.User;
            var userId = userContext.GetUserId();

            //authorize for read and create operations
            if (requirement.ResourceOperation is OperationType.Read or OperationType.Create)
            {
                context.Succeed(requirement);
                return;
            }

            //authorize for creator
            if (resource is Category or Question)
            {
                if (resource.CreatorId == userId)
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            var user = await _dbContext.Users.FindAsync(userId);

            //authorize for admin
            if (user?.RoleId == 1)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
