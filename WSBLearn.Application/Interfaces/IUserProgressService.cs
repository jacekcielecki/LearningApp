using WSBLearn.Application.Responses;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserProgressService
    {
        QuizCompletedResponse CompleteQuiz(int userId, int categoryId, string quizLevelName, int expGained);
        void CompleteAchievement(int userId, int achievementId);
    }
}
