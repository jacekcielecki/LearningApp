using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.CategoryGroup;

namespace WSBLearn.Application.Interfaces
{
    public interface ICategoryGroupService
    {
        Task<List<CategoryGroupDto>> GetAllAsync();
        Task<CategoryGroupDto> GetByIdAsync(int id);
        Task<CategoryGroupDto> CreateAsync(CreateCategoryGroupRequest request);
        Task<CategoryGroupDto> UpdateAsync(int id, UpdateCategoryGroupRequest request);
        Task<CategoryGroupDto> AddCategoryAsync(int id, int categoryId);
        Task<CategoryGroupDto> RemoveCategoryAsync(int id, int categoryId);
        Task DeleteAsync(int id);
    }
}
