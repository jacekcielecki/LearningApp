using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        public IEnumerable<Question> GetQuestions()
        {
            return new List<Question>()
            {
            };
        }
    }
}
