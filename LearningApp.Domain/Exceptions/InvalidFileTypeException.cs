namespace LearningApp.Domain.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException() : base("File extension must be .jpg, .svg or .png")
        {
        }
    }
}
