﻿using LearningApp.Application.Dtos;
using LearningApp.Application.Services;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Tests.Services
{
    public class UserProgressServiceTests
    {
        private readonly WsbLearnDbContext _dbContext;

        public UserProgressServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<WsbLearnDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new WsbLearnDbContext(dbContextOptions);
        }

        [Fact]
        public async Task CompleteQuizAsync_WithValidParameters_UpdatesUserProgress()
        {
            //arrange
            var quizLevelName = "Easy";
            var level = 1;
            var expGained = 10;

            var existingCategory = new Category { Name = "TestCategoryName" };
            await _dbContext.Categories.AddAsync(existingCategory);
            await _dbContext.SaveChangesAsync();

            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                Role = new Role { Name = "TestUserRoleName" },
                UserProgress = new UserProgress
                {
                    ExperiencePoints = 0,
                    TotalCompletedQuiz = 0,
                    Level = level,
                    CategoryProgress = new List<CategoryProgress>
                    {
                        new CategoryProgress
                        {
                            CategoryName = existingCategory.Name,
                            CategoryId = existingCategory.Id,
                            CategoryCompleted = false,
                            LevelProgresses = new List<LevelProgress>
                            {
                                new LevelProgress
                                {
                                    LevelName = quizLevelName,
                                    QuizzesToFinish = 2,
                                    FinishedQuizzes = 0,
                                    LevelCompleted = false
                                }
                            }
                        }
                    }
                }
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var service = new UserProgressService(_dbContext);

            //act
            var result = await service.CompleteQuizAsync(existingUser.Id, existingCategory.Id, quizLevelName, expGained);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QuizCompletedDto>();
            result.TotalExperiencePoints.Should().BeGreaterOrEqualTo(expGained);
            result.TotalCompletedQuiz.Should().BeGreaterOrEqualTo(1);
            result.CurrentUserLevel.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task CompleteAchievementAsync_WithValidUserIdAndAchievementId_UpdatesUserAchievements()
        {
            //arrange
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword"
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var existingUserProgress = new UserProgress { UserId = existingUser.Id };
            var existingAchievement = new Achievement { Name = "TestAchievementName" };
            await _dbContext.UserProgresses.AddAsync(existingUserProgress);
            await _dbContext.Achievements.AddAsync(existingAchievement);
            await _dbContext.SaveChangesAsync();

            var service = new UserProgressService(_dbContext);

            //act
            await service.CompleteAchievementAsync(existingUser.Id, existingAchievement.Id);

            //assert
            var check = await _dbContext.Users
                .Include(x => x.UserProgress)
                .ThenInclude(x => x.Achievements)
                .FirstOrDefaultAsync(x => x.Id == existingUser.Id);

            check.Should().NotBeNull();
            check?.UserProgress.Achievements
                .FirstOrDefault(x => x.Id == existingAchievement.Id).Should().NotBeNull();
        }
    }
}
