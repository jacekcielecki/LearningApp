using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Question;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class QuestionControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public QuestionControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<WsbLearnDbContext>));

                    if (dbContextOptions is not null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<WsbLearnDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task GetAllByCategoryAsync_WithValidCategoryId_ReturnsItemsById()
        {
            //arrange
            var existingCategory = new Category
            {
                Name = "TestCategoryName"
            };
            var existingQuestion = new Question
            {
                QuestionContent = "TestQuestionContent",
                CategoryId = existingCategory.Id
            };
            await _databaseSeeder.Seed(existingCategory);
            await _databaseSeeder.Seed(existingQuestion);

            //act
            var response = await _client.GetAsync($"api/Question/{existingQuestion.CategoryId}");

            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<QuestionDto>>().Should().BeOfType<List<QuestionDto>>();
        }

        [Fact]
        public async Task GetAllByLevelAsync_WithValidCategoryIdAndLevel_ReturnsItemsByIdAndLevel()
        {
            //arrange
            var existingCategory = new Category
            {
                Name = "TestCategoryName",
            };
            var existingQuestion = new Question
            {
                QuestionContent = "TestQuestionContent",
                CategoryId = existingCategory.Id,
                Level = 3
            };
            await _databaseSeeder.Seed(existingCategory);
            await _databaseSeeder.Seed(existingQuestion);

            //act
            var response = await _client.GetAsync($"api/Question/all/{existingCategory.Id}/{existingQuestion.Level}");

            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<QuestionDto>>().Should().BeOfType<List<QuestionDto>>();
        }

        [Fact]
        public async Task GetQuizAsync_WithValidCategoryIdAndLevel_ReturnsQuiz()
        {
            //arrange
            var existingCategory = new Category
            {
                Name = "TestCategoryName",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            var existingQuestion = new Question
            {
                QuestionContent = "TestQuestionContent",
                CategoryId = existingCategory.Id,
                Level = 2
            };
            var existingUser = new User
            {
                Id = 999,
                EmailAddress = "testUser@mail.com",
                Password = "testUserPassword",
                Username = "testUserUsername"
            };
            var existingUserProgress = new UserProgress
            {
                UserId = existingUser.Id,
            };
            var existingCategoryProgress = new CategoryProgress
            {
                CategoryId = existingCategory.Id,
                UserProgressId = existingUserProgress.Id,
                CategoryName = existingCategory.Name
            };

            await _databaseSeeder.Seed(existingCategory);
            await _databaseSeeder.Seed(existingQuestion);
            await _databaseSeeder.Seed(existingUser);
            await _databaseSeeder.Seed(existingUserProgress);
            await _databaseSeeder.Seed(existingCategoryProgress);

            //act
            var response = await _client.GetAsync($"api/Question/{existingCategory.Id}/{existingQuestion.Level}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<QuestionDto>>().Should().BeOfType<List<QuestionDto>>();
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreateAndCategoryId_ReturnsStatusOk()
        {
            //arrange
            var existingCategory = new Category
            {
                Name = "TestCategoryName",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            await _databaseSeeder.Seed(existingCategory);

            var itemToCreate = new CreateQuestionRequest
            {
                QuestionContent = "Test QuestionContent 1",
                A = "Test answer a",
                B = "Test answer b",
                CorrectAnswer = 'a',
                Level = 1
            };

            //act
            var response = await _client.PostAsync($"api/Question/{existingCategory.Id}", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<QuestionDto>().Should().BeOfType<QuestionDto>();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdateAndQuestionId_ReturnsStatusOk()
        {
            //arrange
            var existingCategory = new Category
            {
                Name = "TestCategoryName"
            };
            var itemToUpdate = new Question
            {
                QuestionContent = "TestQuestionContent",
                CategoryId = existingCategory.Id
            };
            await _databaseSeeder.Seed(existingCategory);
            await _databaseSeeder.Seed(itemToUpdate);

            var updatedItem = new UpdateQuestionRequest
            {
                QuestionContent = "Updated Test QuestionContent 4",
                A = "Test answer a",
                B = "Test answer b",
                CorrectAnswer = 'a',
                Level = 1,
                CategoryId = existingCategory.Id,
            };

            //act
            var response = await _client.PutAsync($"api/Question/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<QuestionDto>().Should().BeOfType<QuestionDto>();
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new Question
            {
                QuestionContent = "TestQuestionContent",
            };
            await _databaseSeeder.Seed(itemToDelete);

            //act
            var response = await _client.DeleteAsync($"api/Question/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
