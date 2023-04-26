namespace LearningApp.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName) : base($"{entityName} with given id not found.")
        {
        }
    }
}