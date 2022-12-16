using WSBLearn.Application.Dtos;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserProgressService
    {
        Task<QuizCompletedDto> CompleteQuizAsync(int userId, int categoryId, string quizLevelName, int expGained);
        Task CompleteAchievementAsync(int userId, int achievementId);
    }
}
