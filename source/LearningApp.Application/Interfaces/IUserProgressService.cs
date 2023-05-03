using LearningApp.Application.Dtos;

namespace LearningApp.Application.Interfaces
{
    public interface IUserProgressService
    {
        Task<QuizCompletedDto> CompleteQuizAsync(int userId, int categoryId, string quizLevelName, int expGained);
        Task CompleteAchievementAsync(int userId, int achievementId);
    }
}
