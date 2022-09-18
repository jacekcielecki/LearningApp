using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        public IEnumerable<Question> GetQuestions()
        {
            return new List<Question>()
            {
                new Question(){ Id = 1, LessonId = 1, LessonLevel=1, ImageUrl="", QuestionContent = "2 + 2 =", A="1", B="4", C="7", D="null", CorrectAnswer="B"}
            };
        }
    }
}
