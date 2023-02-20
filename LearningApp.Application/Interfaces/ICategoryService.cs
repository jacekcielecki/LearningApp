using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Category;

namespace LearningApp.Application.Interfaces
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
