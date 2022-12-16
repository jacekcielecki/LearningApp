using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Dtos;
using WSBLearn.Dal.Persistence;

namespace WSBLearn.Application.Services
{
    public class UserProgressService : IUserProgressService
    {
        private readonly WsbLearnDbContext _dbContext;

        public UserProgressService(WsbLearnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<QuizCompletedDto> CompleteQuizAsync(int userId, int categoryId, string quizLevelName, int expGained)
        {
            if (expGained is < 0 or > 20)
                throw new ArgumentException("ExpGained need to be between 0 and 20");
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User with given id not Found");
            var userCategoryProgress = user.UserProgress.CategoryProgress.FirstOrDefault(e => e.CategoryId == categoryId);
            if (userCategoryProgress is null)
                throw new NotFoundException("User with given id has not started any quiz in this category");
            var userLevelProgress =
                userCategoryProgress.LevelProgresses.FirstOrDefault(e => e.LevelName == quizLevelName);
            if (userLevelProgress is null)
                throw new ValidationException("Something went wrong");

            user.UserProgress.ExperiencePoints += expGained;
            user.UserProgress.TotalCompletedQuiz++;
            user.UserProgress.Level = DetermineUserLevel(user.UserProgress.ExperiencePoints);
            userLevelProgress.FinishedQuizzes++;
            if (userLevelProgress.FinishedQuizzes >= userLevelProgress.QuizzesToFinish)
                userLevelProgress.LevelCompleted = true;
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

        public async Task CompleteAchievementAsync(int userId, int achievementId)
        {
            var user =await _dbContext.Users
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.Achievements)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User with given id not found");
            var achievement = _dbContext.Achievements.FirstOrDefault(a => a.Id == achievementId);
            if (achievement is null)
                throw new NotFoundException("Achievement with given id not found");
            
            user.UserProgress.Achievements.Add(achievement);

            await _dbContext.SaveChangesAsync();
        }

        private int DetermineUserLevel(int experiencePoints)
        {
            var userLevel = 1;
            var levelThreshold = 120;
            while (experiencePoints > levelThreshold)
            {
                userLevel ++;
                levelThreshold = levelThreshold * 2;
            }

            return userLevel;
        }
    }
}
