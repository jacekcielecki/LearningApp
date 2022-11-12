using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Category;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;

        public CategoryService(WsbLearnDbContext dbContext, ILogger<CategoryService> logger, IMapper mapper, 
            IValidator<CreateCategoryRequest> createCategoryRequestValidator, IValidator<UpdateCategoryRequest> updateCategoryRequestValidator)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
        }

        public int? Create(CreateCategoryRequest createCategoryRequest)
        {
            ValidationResult validationResult = _createCategoryRequestValidator.Validate(createCategoryRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

            Category category = _mapper.Map<Category>(createCategoryRequest);
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category.Id;
        }

        public IEnumerable<CategoryDto>? GetAll()
        {
            IEnumerable<Category> categories = _dbContext.Categories.Include(r => r.Questions).AsEnumerable();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoryDtos;
        }

        public CategoryDto GetById(int id)
        {
            Category? category = _dbContext.Categories.Include(r => r.Questions).FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }

        public CategoryDto Update(int id, UpdateCategoryRequest updateCategoryRequest)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            ValidationResult validationResult = _updateCategoryRequestValidator.Validate(updateCategoryRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

            category.Name = updateCategoryRequest.Name;
            category.Description = updateCategoryRequest.Description;
            category.IconUrl = updateCategoryRequest.IconUrl;
            category.QuestionsPerLesson = updateCategoryRequest.QuestionsPerLesson;
            category.LessonsPerLevel = updateCategoryRequest.LessonsPerLevel;

            var categoryDto = _mapper.Map<CategoryDto>(category);
            _dbContext.SaveChanges();

            return categoryDto;
        }

        public void Delete(int id)
        {
            Category? category = _dbContext.Categories.Include(r => r.Questions).FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return;
        }
    }
}
