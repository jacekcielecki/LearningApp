using AutoMapper;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
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

        public int? Create(CategoryDto categoryDto)
        {
            Category category = _mapper.Map<Category>(categoryDto);
            try
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError(Messages.DataAccessError);
                return null;
            }

            return category.Id;
        }

        public IEnumerable<CategoryDto>? GetAll()
        {
            IEnumerable<Category> categories = _dbContext.Categories.AsEnumerable();
            var result = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return result;
        }

        public CategoryDto GetById(int id)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            var result = _mapper.Map<CategoryDto>(category);

            return result;
        }

        public CategoryDto? Update(int id, CategoryDto categoryDto)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.IconUrl = categoryDto.IconUrl;
            category.Levels = categoryDto.Levels;
            category.QuestionsPerLevel = categoryDto.QuestionsPerLevel;

            var result = _mapper.Map<CategoryDto>(category);
            //_dbContext.Categories.Update(category);
            _dbContext.SaveChanges();

            return result;
        }

        public void Delete(int id)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return;
        }
    }
}
