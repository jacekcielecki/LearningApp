﻿using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Category;
using LearningApp.Application.Services;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System.Security.Claims;

namespace LearningApp.Application.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IValidator<CreateCategoryRequest> _createCategoryValidator = new CreateCategoryRequestValidator();
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator = new UpdateCategoryRequestValidator();
        private readonly Mock<IAuthorizationService> _authorizationService = new Mock<IAuthorizationService>();

        public CategoryServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LearningAppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryServiceTests")
                .Options;

            _dbContext = new LearningAppDbContext(dbContextOptions);
            _authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success());
        }

        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsAllItems()
        {
            //arrange
            var existingItems = new List<Category>
            {
                new() { Name = "TestCategory" },
                new() { Name = "TestCategory" },
                new() { Name = "TestCategory" }
            };
            await _dbContext.Categories.AddRangeAsync(existingItems);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryService(_dbContext, AutoMapperSingleton.Mapper, _createCategoryValidator, _updateCategoryValidator, _authorizationService.Object);

            //act
            var result = await service.GetAllAsync(FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<CategoryDto>>();
            result.Should().ContainEquivalentOf(existingItems.FirstOrDefault(), options => 
                options.ComparingByMembers<CategoryDto>()
                    .ExcludingMissingMembers()
                    .Excluding(x => x!.CategoryGroups)
                    .Excluding(x => x!.Questions));
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingItem_ReturnsItemById()
        {
            //arrange
            var existingItem = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryService(_dbContext, AutoMapperSingleton.Mapper, _createCategoryValidator, _updateCategoryValidator, _authorizationService.Object);

            //act
            var result = await service.GetByIdAsync(existingItem.Id, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Should().BeEquivalentTo(existingItem,
                options => options.ComparingByMembers<CategoryDto>()
                    .ExcludingMissingMembers()
                    .Excluding(x => x!.CategoryGroups)
                    .Excluding(x => x!.Questions));
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsCreatedItem()
        {
            //arrange
            var itemToCreate = new CreateCategoryRequest
            {
                Name = "TestCategory", 
                Description = "TestCategoryDescription", 
                QuizPerLevel = 5,
                QuestionsPerQuiz = 6
            };

            var service = new CategoryService(_dbContext, AutoMapperSingleton.Mapper, _createCategoryValidator, _updateCategoryValidator, _authorizationService.Object);

            //act
            var result = await service.CreateAsync(itemToCreate, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Should().BeEquivalentTo(itemToCreate,
                options => options.ComparingByMembers<CategoryDto>()
                    .ExcludingMissingMembers());
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsUpdatedItem()
        {
            //arrange
            var existingItem = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var itemToUpdate = new UpdateCategoryRequest
            {
                Name = "UpdatedTestCategory",
                Description = "UpdatedTestCategoryDescription",
                QuizPerLevel = 5,
                QuestionsPerQuiz = 6
            };

            var service = new CategoryService(_dbContext, AutoMapperSingleton.Mapper, _createCategoryValidator, _updateCategoryValidator, _authorizationService.Object);

            //act
            var result = await service.UpdateAsync(existingItem.Id, itemToUpdate, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Id.Should().Be(existingItem.Id);
            result.Should().BeEquivalentTo(itemToUpdate,
                options => options.ComparingByMembers<CategoryDto>()
                    .ExcludingMissingMembers());
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_DeletesItem()
        {
            //arrange
            var existingItem = new Category { Name = "TestCategory" };
            await _dbContext.Categories.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryService(_dbContext, AutoMapperSingleton.Mapper, _createCategoryValidator, _updateCategoryValidator, _authorizationService.Object);

            //act
            await service.DeleteAsync(existingItem.Id, FakeHttpContextSingleton.ClaimsPrincipal);

            //assert
            var deletedItem = _dbContext.Achievements.FirstOrDefault(x => x.Id == existingItem.Id);
            deletedItem.Should().BeNull();
        }
    }
}
