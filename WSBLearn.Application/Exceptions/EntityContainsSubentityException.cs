namespace WSBLearn.Application.Exceptions
{
    public class EntityContainsSubentityException : Exception
    {
        public EntityContainsSubentityException(string message) : base(message)
        {
        }
    }
}