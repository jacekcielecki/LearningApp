using LearningApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LearningApp.Application.Authorization
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public OperationType ResourceOperation { get; }

        public ResourceOperationRequirement(OperationType operation)
        {
            ResourceOperation = operation;
        }
    }
}
