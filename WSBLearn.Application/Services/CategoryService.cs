using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(WsbLearnDbContext dbContext, ILogger<CategoryService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public int? Create(CreateCategoryRequest createCategoryRequest)
        {
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
            if (category.Questions.Any())
                throw new NotFoundException(string.Format(Messages.ExistingSubentity, "Category", "Question"));

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return;
        }
    }
}
