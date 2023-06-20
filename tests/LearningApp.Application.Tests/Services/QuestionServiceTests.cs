using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Question;
using LearningApp.Application.Services;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System.Security.Claims;

namespace LearningApp.Application.Tests.Services
{
    public class QuestionServiceTests
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IValidator<CreateQuestionRequest> _createQuestionValidator = new CreateQuestionRequestValidator();
        private readonly IValidator<UpdateQuestionRequest> _updateQuestionValidator = new UpdateQuestionRequestValidator();
        private readonly Mock<IAuthorizationService> _authorizationServiceStub = new Mock<IAuthorizationService>();

        public QuestionServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LearningAppDbContext>()
                .UseInMemoryDatabase(databaseName: "QuestionServiceTests")
                .Options;

            _dbContext = new LearningAppDbContext(dbContextOptions);
            _authorizationServiceStub.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success());
        }

        [Fact]
        public async Task GetAllByCategoryAsync_WithExistingItems_ReturnsAllItemsByCategory()
        {
            //arrange
            var existingCategory = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingCategory);

            var existingQuestions = new List<Question>
            {
                new() { QuestionContent = "TestContent", CorrectAnswer = 'a', CategoryId = existingCategory.Id },
                new() { QuestionContent = "TestContent", CorrectAnswer = 'b', CategoryId = existingCategory.Id },
                new() { QuestionContent = "TestContent", CorrectAnswer = 'c', CategoryId = 1232143 }
            };
            await _dbContext.Questions.AddRangeAsync(existingQuestions);
            await _dbContext.SaveChangesAsync();

            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            var result = await service.GetAllByCategoryAsync(existingCategory.Id, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<QuestionDto>>();
            result.Should().ContainEquivalentOf(existingQuestions
                .FirstOrDefault(x => x.CategoryId == existingCategory.Id), options => 
                options.ComparingByMembers<QuestionDto>().ExcludingMissingMembers());
            result.Where(x => x.CategoryId != existingCategory.Id).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllByLevelAsync_WithExistingItems_ReturnsAllItemsByLevel()
        {
            //arrange
            var testLevel = 1;
            var existingCategory = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingCategory);

            var existingQuestions = new List<Question>
            {
                new() { QuestionContent = "TestContent", CorrectAnswer = 'a', CategoryId = existingCategory.Id, Level = 1 },
                new() { QuestionContent = "TestContent", CorrectAnswer = 'b', CategoryId = existingCategory.Id, Level = 2 },
                new() { QuestionContent = "TestContent", CorrectAnswer = 'c', CategoryId = 1232143, Level = 3 }
            };
            await _dbContext.Questions.AddRangeAsync(existingQuestions);
            await _dbContext.SaveChangesAsync();

            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            var result = await service.GetAllByLevelAsync(existingCategory.Id, testLevel, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<QuestionDto>>();
            result.Should().ContainEquivalentOf(existingQuestions
                .FirstOrDefault(x => x.CategoryId == existingCategory.Id && x.Level == testLevel), options =>
                options.ComparingByMembers<QuestionDto>().ExcludingMissingMembers()); 
            result.Where(x => x.CategoryId != existingCategory.Id && x.Level != testLevel).Should().BeEmpty();
        }

        [Fact]
        public async Task GetQuizAsync_WithExistingItems_ReturnsQuiz()
        {
            //arrange
            var existingCategory = new Category { Name = "TestCategoryName", QuizPerLevel = 5, QuestionsPerQuiz = 5 };
            await _dbContext.Categories.AddAsync(existingCategory);
            await _dbContext.SaveChangesAsync();

            var existingQuestion = new Question { QuestionContent = "TestQuestionContent", CategoryId = existingCategory.Id, Level = 2 };
            await _dbContext.Questions.AddAsync(existingQuestion);
            await _dbContext.SaveChangesAsync();

            var existingUser = new User { EmailAddress = "testUser@mail.com", Password = "testUserPassword", Username = "testUserUsername" };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var existingUserProgress = new UserProgress { UserId = 1 };
            await _dbContext.UserProgresses.AddAsync(existingUserProgress);
            await _dbContext.SaveChangesAsync();

            var existingCategoryProgress = new CategoryProgress { CategoryId = existingCategory.Id, UserProgressId = existingUserProgress.Id, CategoryName = existingCategory.Name };
            await _dbContext.CategoryProgresses.AddAsync(existingCategoryProgress);
            await _dbContext.SaveChangesAsync();

            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            var result = await service.GetQuizAsync(existingCategory.Id, existingQuestion.Level, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<QuestionDto>>();
            result.Should().ContainEquivalentOf(existingQuestion, options =>
                options.ComparingByMembers<QuestionDto>().ExcludingMissingMembers());
            result.Where(x => x.CategoryId != existingCategory.Id && x.Level != existingQuestion.Level).Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsCreatedItem()
        {
            //arrange
            var existingCategory = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingCategory);

            var itemToCreate = new CreateQuestionRequest
            {
                QuestionContent = "TestContent",
                A = "TestAnswerA",
                B = "TestAnswerB",
                CorrectAnswer = 'a',
                Level = 1
            };
            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            var result = await service.CreateAsync(itemToCreate, existingCategory.Id, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QuestionDto>();
            result.Should().BeEquivalentTo(itemToCreate,
                options => options.ComparingByMembers<QuestionDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsUpdatedItem()
        {
            //arrange
            var existingCategory = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingCategory);
            await _dbContext.SaveChangesAsync();

            var existingQuestion = new Question
            {
                QuestionContent = "TestContent",
                CorrectAnswer = 'a',
                CategoryId = existingCategory.Id,
                Level = 1
            };
            await _dbContext.Questions.AddAsync(existingQuestion);
            await _dbContext.SaveChangesAsync();

            var itemToUpdate = new UpdateQuestionRequest
            {
                QuestionContent = "UpdatedTestContent",
                A = "UpdatedTestAnswerA",
                B = "UpdatedTestAnswerB",
                CorrectAnswer = 'b',
                Level = 2,
                CategoryId = existingCategory.Id
            };

            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            var result = await service.UpdateAsync(existingQuestion.Id, itemToUpdate, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QuestionDto>();
            result.Id.Should().Be(existingQuestion.Id);
            result.Should().BeEquivalentTo(itemToUpdate,
                options => options.ComparingByMembers<QuestionDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_DeletesItem()
        {
            //arrange
            var existingQuestion = new Question { QuestionContent = "TestContent", CorrectAnswer = 'a' };
            await _dbContext.Questions.AddAsync(existingQuestion);
            await _dbContext.SaveChangesAsync();

            var service = new QuestionService(_dbContext, AutoMapperSingleton.Mapper, _createQuestionValidator, _updateQuestionValidator, _authorizationServiceStub.Object);

            //act
            await service.DeleteAsync(existingQuestion.Id, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            var deletedItem = _dbContext.Questions.FirstOrDefault(x => x.Id == existingQuestion.Id);
            deletedItem.Should().BeNull();
        }
    }
}
