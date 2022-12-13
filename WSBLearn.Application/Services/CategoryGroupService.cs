using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.CategoryGroup;
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

        public async Task<List<CategoryGroupDto>> GetAllAsync()
        {
            var entities = await _dbContext.CategoryGroups
                .Include(e => e.Categories)
                .ToListAsync();

            return _mapper.Map<List<CategoryGroupDto>>(entities);
        }

        public async Task<CategoryGroupDto> GetByIdAsync(int id)
        {
            var entity = await _dbContext.CategoryGroups
                .Include(e => e.Categories)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public async Task<CategoryGroupDto> CreateAsync(CreateCategoryGroupRequest request)
        {
            var validationResult = await _createCategoryGroupRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var entity = _mapper.Map<CategoryGroup>(request);
            await _dbContext.CategoryGroups.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public async Task<CategoryGroupDto> UpdateAsync(int id, UpdateCategoryGroupRequest request)
        {
            var entity = await _dbContext.CategoryGroups.FindAsync(id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");
            var validationResult = await _updateCategoryGroupRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            entity.Name = request.Name;
            entity.IconUrl = request.IconUrl;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public async Task<CategoryGroupDto> AddCategoryAsync(int id, int categoryId)
        {
            var entity = await _dbContext.CategoryGroups
                .Include(e => e.Categories)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");
            var category = _dbContext.Categories.FirstOrDefault(e => e.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category with given id not found");

            entity.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public async Task<CategoryGroupDto> RemoveCategoryAsync(int id, int categoryId)
        {
            var entity = await _dbContext.CategoryGroups
                .Include(e => e.Categories)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
                throw new NotFoundException("Category with given id not found");

            entity.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryGroupDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.CategoryGroups.FindAsync(id);
            if (entity is null)
                throw new NotFoundException("CategoryGroup with given id not found");

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}