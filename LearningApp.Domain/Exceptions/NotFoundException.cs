using LearningApp.Domain.Common;

namespace LearningApp.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName) : base(Messages.InvalidId(entityName))
        {
        }
    }
}