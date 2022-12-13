using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.Category;

namespace WSBLearn.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>?> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest createCategoryRequest);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest updateCategoryRequest);
        Task DeleteAsync(int id);
    }
}
