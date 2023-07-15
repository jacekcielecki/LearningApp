using LearningApp.Application.Extensions;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Enums;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;

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
            var userContext = context?.User;
            var userRole = userContext.GetUserRole();
            var userId = userContext.GetUserId();

            if (requirement.ResourceOperation == OperationType.Read)
            {
                context?.Succeed(requirement);
                return Task.CompletedTask;
            }
            if (userContext is null)
            {
                return Task.CompletedTask;
            }
            if (userRole == "Admin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            switch (resource)
            {
                case Category:
                {
                    var isResourceCreator = resource.CreatorId == userId;
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
                        var isResourceCategoryCreator = resourceCategory.CreatorId == userId;
                        if (isResourceCategoryCreator) context.Succeed(requirement);
                    }
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
