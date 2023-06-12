using LearningApp.Application.Dtos;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class UserProgressControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public UserProgressControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<LearningAppDbContext>));

                    if (dbContextOptions is not null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<LearningAppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task CompleteQuizAsync_WithValidQueryParameters_ReturnsStatusOk()
        {
            //arrange
            var quizLevelName = "Easy";
            var level = 1;
            var expGained = 10;

            var existingCategory = new Category { Name = "TestCategoryName" };
            await _databaseSeeder.Seed(existingCategory);

            var existingUser = new User
            {
                Id = 999,
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
            await _databaseSeeder.Seed(existingUser);

            //act
            var response = 
                await _client
                .PatchAsync($"api/UserProgress/{existingCategory.Id}/{level}/{quizLevelName}/{expGained}", null);
            
            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<QuizCompletedDto>().Should().BeOfType<QuizCompletedDto>();
        }

        [Fact]
        public async Task CompleteAchievementAsync_WithValidUserIdAndAchievementId()
        {
            //arrange
            var existingUser = new User
            {
                Id = 999,
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword"
            };
            var existingAchievement = new Achievement {Name = "TestAchievementName" };
            await _databaseSeeder.Seed(existingUser);
            await _databaseSeeder.Seed(existingAchievement);

            //act
            var response =
                await _client.PatchAsync(
                    $"/api/UserProgress/CompleteAchievement/{existingUser.Id}/{existingAchievement.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
