using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Category;
using System.Security.Claims;

namespace LearningApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest createCategoryRequest, int userId);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest updateCategoryRequest, ClaimsPrincipal use);
        Task DeleteAsync(int id);
    }
}
