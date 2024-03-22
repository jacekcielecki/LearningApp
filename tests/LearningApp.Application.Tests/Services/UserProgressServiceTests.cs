using LearningApp.Application.Dtos;
using LearningApp.Application.Services;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace LearningApp.Application.Tests.Services
{
    public class UserProgressServiceTests
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly Mock<IAuthorizationService> _authorizationServiceStub = new Mock<IAuthorizationService>();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        public UserProgressServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LearningAppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserProgressServiceTests")
                .Options;

            _dbContext = new LearningAppDbContext(dbContextOptions);
            _authorizationServiceStub.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success());
            _httpContextAccessorMock.Setup(x => x.HttpContext)
                .Returns(new DefaultHttpContext { User = FakeHttpContextSingleton.ClaimsPrincipal });
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
                Role = new Role { Name = "Admin", Id = 2 },
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
                                    QuizToFinish = 2,
                                    FinishedQuiz = 0,
                                    LevelCompleted = false
                                }
                            }
                        }
                    }
                }
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var service = new UserProgressService(_dbContext, _httpContextAccessorMock.Object);

            //act
            var result = await service.CompleteQuizAsync(existingCategory.Id, quizLevelName, expGained);

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
            var existingUserDto = new UserDto
            {
                Id = 1,
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
            };

            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var existingUserProgress = new UserProgress { UserId = existingUser.Id };
            await _dbContext.UserProgresses.AddAsync(existingUserProgress);
            await _dbContext.SaveChangesAsync();

            var existingAchievement = new Achievement { Name = "TestAchievementName" };
            await _dbContext.Achievements.AddAsync(existingAchievement);
            await _dbContext.SaveChangesAsync();

            var service = new UserProgressService(_dbContext, _httpContextAccessorMock.Object);

            //act
            await service.CompleteAchievementAsync(existingAchievement.Id, existingUserDto);

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
