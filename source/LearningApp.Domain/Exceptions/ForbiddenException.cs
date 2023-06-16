using LearningApp.Domain.Common;

namespace LearningApp.Domain.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base(Messages.UnauthorizedAccess)
        {
            
        }
    }
}
