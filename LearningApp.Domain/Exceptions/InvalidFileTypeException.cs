namespace LearningApp.Application.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException(string message) : base(message)
        {
        }
    }
}
