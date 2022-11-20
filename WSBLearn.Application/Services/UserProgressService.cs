using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Responses;
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

        public QuizCompletedResponse CompleteQuiz(int userId, int categoryId, string quizLevelName, int expGained)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefault(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User with given id not Found");

            var userCategoryProgress =
                user.UserProgress.CategoryProgress.FirstOrDefault(cp => cp.CategoryId == categoryId);
            if (userCategoryProgress is null)
                throw new NotFoundException("User with given id has not started any quiz in this category");

            var userLevelProgress =
                userCategoryProgress.LevelProgresses.FirstOrDefault(lp => lp.LevelName == quizLevelName);
            if (userLevelProgress is null)
                throw new ValidationException("Something went wrong");

            user.UserProgress.ExperiencePoints += expGained;
            user.UserProgress.TotalCompletedQuiz++;
            user.UserProgress.Level = DetermineUserLevel(user.UserProgress.ExperiencePoints);
            userLevelProgress.FinishedQuizzes++;
            if (userLevelProgress.FinishedQuizzes >= userLevelProgress.QuizzesToFinish)
                userLevelProgress.LevelCompleted = true;

            _dbContext.SaveChanges();

            var quizCompletedResponse = new QuizCompletedResponse()
            {
                TotalExperiencePoints = user.UserProgress.ExperiencePoints,
                TotalCompletedQuiz = user.UserProgress.TotalCompletedQuiz,
                CurrentUserLevel = user.UserProgress.Level,
                IsCategoryLevelCompleted = userLevelProgress.LevelCompleted
            };

            return quizCompletedResponse; 
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
