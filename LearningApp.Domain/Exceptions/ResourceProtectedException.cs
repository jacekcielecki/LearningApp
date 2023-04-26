using LearningApp.Domain.Common;

namespace LearningApp.Domain.Exceptions
{
    public class ResourceProtectedException : Exception
    {
        public ResourceProtectedException() : base(ErrorMessages.AccessProtectedResource)
        {
        }
    }
}
