using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public interface IQuestionService
    {
        IEnumerable<Question> GetQuestions();
    }
}