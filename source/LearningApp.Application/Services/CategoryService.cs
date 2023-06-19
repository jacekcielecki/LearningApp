using AutoMapper;
using FluentValidation;
using LearningApp.Application.Authorization;
using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Category;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Enums;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;
        private readonly IAuthorizationService _authorizationService;

        public CategoryService(LearningAppDbContext dbContext, IMapper mapper,
            IValidator<CreateCategoryRequest> createCategoryRequestValidator, 
            IValidator<UpdateCategoryRequest> updateCategoryRequestValidator, 
            IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
            _authorizationService = authorizationService;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            IEnumerable<Category> entities = await _dbContext.Categories
                .Include(r => r.Questions)
                .ToListAsync();

            return _mapper.Map<List<CategoryDto>>(entities);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
                throw new NotFoundException(nameof(Category));

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest createCategoryRequest, int userId)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            var entity = _mapper.Map<Category>(createCategoryRequest);
            entity.CreatorId = userId;
            entity.DateCreated = DateTime.Now;
            await _dbContext.Categories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest request, ClaimsPrincipal user)
        {
            var entity = await _dbContext.Categories.FindAsync(id);
            if (entity is null) throw new NotFoundException(nameof(Category));

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, entity, new ResourceOperationRequirement(OperationType.Update));
            if (!authorizationResult.Succeeded) throw new ForbiddenException();
            
            var validationResult = await _updateCategoryRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IconUrl = request.IconUrl;
            entity.QuestionsPerQuiz = request.QuestionsPerQuiz;
            entity.QuizPerLevel = request.QuizPerLevel;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefault(c => c.Id == id);
            if (entity is null)
                throw new NotFoundException(nameof(Category));

            _dbContext.Categories.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
