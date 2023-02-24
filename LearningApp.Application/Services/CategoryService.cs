using AutoMapper;
using FluentValidation;
using LearningApp.Application.Common;
using LearningApp.Application.Dtos;
using LearningApp.Application.Exceptions;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Category;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;

        public CategoryService(WsbLearnDbContext dbContext, IMapper mapper,
            IValidator<CreateCategoryRequest> createCategoryRequestValidator, IValidator<UpdateCategoryRequest> updateCategoryRequestValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
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
                throw new NotFoundException(string.Format(ValidationExceptions.InvalidId, "Category"));

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest createCategoryRequest)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            var entity = _mapper.Map<Category>(createCategoryRequest);
            await _dbContext.Categories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var entity = await _dbContext.Categories.FindAsync(id);
            if (entity is null)
                throw new NotFoundException(string.Format(ValidationExceptions.InvalidId, "Category"));
            var validationResult = await _updateCategoryRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IconUrl = request.IconUrl;
            entity.QuestionsPerLesson = request.QuestionsPerLesson;
            entity.LessonsPerLevel = request.LessonsPerLevel;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefault(c => c.Id == id);
            if (entity is null)
                throw new NotFoundException(string.Format(ValidationExceptions.InvalidId, "Category"));

            _dbContext.Categories.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
