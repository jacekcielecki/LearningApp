using WSBLearn.Application.Responses;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserProgressService
    {
        Task<QuizCompletedResponse> CompleteQuizAsync(int userId, int categoryId, string quizLevelName, int expGained);
        Task CompleteAchievementAsync(int userId, int achievementId);
    }
}
