using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Category;
using WSBLearn.Application.Requests.CategoryGroup;
using WSBLearn.Application.Validators.Category;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class CategoryGroupService : ICategoryGroupService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryGroupRequest> _createCategoryGroupRequestValidator;
        private readonly IValidator<UpdateCategoryGroupRequest> _updateCategoryGroupRequestValidator;

        public CategoryGroupService(WsbLearnDbContext dbContext, IMapper mapper, 
            IValidator<CreateCategoryGroupRequest> createCategoryGroupRequestValidator, IValidator<UpdateCategoryGroupRequest> updateCategoryGroupRequestValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createCategoryGroupRequestValidator = createCategoryGroupRequestValidator;
            _updateCategoryGroupRequestValidator = updateCategoryGroupRequestValidator;
        }

        public int Create(CreateCategoryGroupRequest request)
        {
            var validationResult = _createCategoryGroupRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            var entity = _mapper.Map<CategoryGroup>(request);
            _dbContext.CategoryGroups.Add(entity);
            _dbContext.SaveChanges();

            return entity.Id;
        }

        public IEnumerable<CategoryGroupDto> GetAll()
        {
            var entities = _dbContext.CategoryGroups.Include(e => e.Categories).AsEnumerable();
            return _mapper.Map<IEnumerable<CategoryGroupDto>>(entities);
        }

        public CategoryGroupDto GetById(int id)
        {
            var entity = _dbContext.CategoryGroups
                .Include(e => e.Categories).FirstOrDefault(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public CategoryGroupDto Update(int id, UpdateCategoryGroupRequest request)
        {
            var entity = _dbContext.CategoryGroups.FirstOrDefault(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");

            var validationResult = _updateCategoryGroupRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            entity.Name = request.Name;
            entity.IconUrl = request.IconUrl;
            _dbContext.SaveChanges();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public void Delete(int id)
        {
            var entity = _dbContext.CategoryGroups.FirstOrDefault(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");

            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }

        public CategoryGroupDto AddCategory(int id, int categoryId)
        {
            var entity = _dbContext.CategoryGroups
                .Include(e => e.Categories).FirstOrDefault(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");
            var category = _dbContext.Categories.FirstOrDefault(e => e.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category with given id not found");

            entity.Categories.Add(category);
            _dbContext.SaveChanges();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public CategoryGroupDto RemoveCategory(int id, int categoryId)
        {
            var entity = _dbContext.CategoryGroups
                .Include(e => e.Categories).FirstOrDefault(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");
            var category = _dbContext.Categories.FirstOrDefault(e => e.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category with given id not found");

            entity.Categories.Remove(category);
            _dbContext.SaveChanges();

            return _mapper.Map<CategoryGroupDto>(entity);
        }
    }
}