using System.Security.Claims;
using LearningApp.Application.Dtos;

namespace LearningApp.Application.Interfaces
{
    public interface IUserProgressService
    {
        Task<QuizCompletedDto> CompleteQuizAsync(ClaimsPrincipal userContext, int categoryId, string quizLevelName, int expGained);
        Task CompleteAchievementAsync(int userId, int achievementId);
    }
}
