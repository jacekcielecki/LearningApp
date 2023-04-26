namespace LearningApp.Domain.Exceptions
{
    public class ResourceProtectedException : Exception
    {
        public ResourceProtectedException(string message) : base(message)
        {
        }
    }
}
