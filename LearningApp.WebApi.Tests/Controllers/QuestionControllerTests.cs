using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Question;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class QuestionControllerTests
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;
        private readonly Mock<IQuestionService> _questionServiceStub = new Mock<IQuestionService>();

        public QuestionControllerTests()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<WsbLearnDbContext>));

                    if (dbContextOptions is not null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<WsbLearnDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
                    services.AddSingleton<IQuestionService>(_questionServiceStub.Object);
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task GetAllByCategoryAsync_WithValidCategoryId_ReturnsItemsById()
        {
            //arrange
            var existingQuestion = new Question
            {
                Id = 1,
                QuestionContent = "Test QuestionContent 1",
                CategoryId = 1
            };
            await _databaseSeeder.Seed(existingQuestion);

            //act
            var response = await _client.GetAsync($"api/Question/{existingQuestion.CategoryId}");

            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllByLevelAsync_WithValidCategoryIdAndLevel_ReturnsItemsByIdAndLevel()
        {
            //arrange
            var existingQuestion = new Question
            {
                Id = 2,
                QuestionContent = "Test QuestionContent 2",
                CategoryId = 2,
                Level = 2
            };
            await _databaseSeeder.Seed(existingQuestion);

            //act
            var response = await _client.GetAsync($"api/Question/all/{existingQuestion.CategoryId}/{existingQuestion.Level}");

            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetQuizAsync_WithValidCategoryIdAndLevel_ReturnsQuiz()
        {
            //arrange
            var existingQuestion = new Question
            {
                Id = 3,
                QuestionContent = "Test QuestionContent 3",
                CategoryId = 3,
                Level = 3
            };
            await _databaseSeeder.Seed(existingQuestion);

            _questionServiceStub
                .Setup(x => x.GetQuizAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<QuestionDto>
                {
                    new()
                    {
                        Id = 1,
                        QuestionContent = "Test Question"
                    }
                });

            //act
            var response = await _client.GetAsync($"api/Question/{existingQuestion.CategoryId}/{existingQuestion.Level}");

            //arrange
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreateAndCategoryId_ReturnsStatusOk()
        {
            //arrange
            var existingCategory = new Category
            {
                Id = 9,
                Name = "TestCategory 9"
            };
            await _databaseSeeder.Seed(existingCategory);

            var itemToCreate = new CreateQuestionRequest
            {
                QuestionContent = "Test QuestionContent 3",
                A = "Test answer a",
                B = "Test answer b",
                CorrectAnswer = 'a',
                Level = 1
            };

            //act
            var response = await _client.PostAsync($"api/Question/{existingCategory.Id}", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdateAndQuestionId_ReturnsStatusOk()
        {
            //arrange
            var existingCategory = new Category
            {
                Id = 8,
                Name = "Test Category 8"
            };
            var itemToUpdate = new Question
            {
                Id = 44,
                QuestionContent = "Test QuestionContent 4",
                CategoryId = 8
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
                CategoryId = 8,
            };

            //act
            var response = await _client.PutAsync($"api/Question/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new Question
            {
                Id = 5,
                QuestionContent = "Test QuestionContent 5",
            };
            await _databaseSeeder.Seed(itemToDelete);

            //act
            var response = await _client.DeleteAsync($"api/Question/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
