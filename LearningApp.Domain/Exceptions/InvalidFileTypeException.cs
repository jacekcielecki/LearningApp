using LearningApp.Domain.Common;

namespace LearningApp.Domain.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException() : base(BlobStorageMessages.InvalidImageExtension)
        {
        }
    }
}
