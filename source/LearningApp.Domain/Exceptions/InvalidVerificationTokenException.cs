using LearningApp.Domain.Common;

namespace LearningApp.Domain.Exceptions
{
    public class InvalidVerificationTokenException : Exception
    {
        public InvalidVerificationTokenException() : base(Messages.InvalidVerificationToken)
        {
            
        }
    }
}
