namespace LearningApp.Domain.Exceptions
{
    public class InvalidVerificationTokenException : Exception
    {
        public InvalidVerificationTokenException(string message) : base(message)
        {
            
        }
    }
}
