using FluentValidation;
using LearningApp.Application.Dtos;
using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using LearningApp.Domain.Common;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Application.Services
{
    public class UserProgressService : IUserProgressService
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProgressService(LearningAppDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<QuizCompletedDto> CompleteQuizAsync(int categoryId, string quizLevelName, int expGained)
        {
            if (expGained is < 0 or > 20) throw new ArgumentException(Messages.InvalidGainedExperience);

            var userContext = _httpContextAccessor.HttpContext?.User;

            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefaultAsync(u => u.Id == userContext.GetUserId());

            if (user is null) throw new NotFoundException(nameof(User));

            var userCategoryProgress = user.UserProgress.CategoryProgress
                .FirstOrDefault(e => e.CategoryId == categoryId);
            if (userCategoryProgress is null) throw new NotFoundException(Messages.QuizNotStarted);

            var userLevelProgress = userCategoryProgress.LevelProgresses.
                FirstOrDefault(e => e.LevelName == quizLevelName);
            if (userLevelProgress is null) throw new ValidationException(Messages.GenericErrorMessage);

            user.UserProgress.ExperiencePoints += expGained;
            user.UserProgress.TotalCompletedQuiz++;
            user.UserProgress.Level = DetermineUserLevel(user.UserProgress.ExperiencePoints);
            userLevelProgress.FinishedQuiz++;
            if (userLevelProgress.FinishedQuiz >= userLevelProgress.QuizToFinish)
            {
                userLevelProgress.LevelCompleted = true;
            }

            if (!userCategoryProgress.CategoryCompleted)
            {
                var allLevelsCompleted = true;
                foreach (var level in userCategoryProgress.LevelProgresses)
                {
                    if (!level.LevelCompleted)
                        allLevelsCompleted = false;
                }
                if (allLevelsCompleted)
                {
                    userCategoryProgress.CategoryCompleted = true;
                    user.UserProgress.TotalCompletedCategory++;
                }
            }
            await _dbContext.SaveChangesAsync();

            var quizCompletedResponse = new QuizCompletedDto()
            {
                TotalExperiencePoints = user.UserProgress.ExperiencePoints,
                TotalCompletedQuiz = user.UserProgress.TotalCompletedQuiz,
                CurrentUserLevel = user.UserProgress.Level,
                IsCategoryLevelCompleted = userLevelProgress.LevelCompleted
            };

            return quizCompletedResponse;
        }

        public async Task CompleteAchievementAsync(int achievementId)
        {
            var userContext = _httpContextAccessor.HttpContext?.User;

            var user = await _dbContext.Users
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.Achievements)
                .FirstOrDefaultAsync(u => u.Id == userContext.GetUserId());
            if (user is null) throw new NotFoundException(nameof(User));

            var achievement = _dbContext.Achievements
                .FirstOrDefault(a => a.Id == achievementId);
            if (achievement is null) throw new NotFoundException(nameof(Achievement));

            user.UserProgress.Achievements
                .Add(achievement);
            await _dbContext.SaveChangesAsync();
        }

        private int DetermineUserLevel(int experiencePoints)
        {
            var userLevel = 1;
            var levelThreshold = 120;
            while (experiencePoints > levelThreshold)
            {
                userLevel++;
                levelThreshold *= 2;
            }

            return userLevel;
        }
    }
}
