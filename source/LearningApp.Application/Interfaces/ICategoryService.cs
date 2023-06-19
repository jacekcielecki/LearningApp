using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Category;
using System.Security.Claims;

namespace LearningApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(ClaimsPrincipal userContext);
        Task<CategoryDto> GetByIdAsync(int id, ClaimsPrincipal userContext);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest createCategoryRequest, ClaimsPrincipal userContext);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest updateCategoryRequest, ClaimsPrincipal userContext);
        Task DeleteAsync(int id, ClaimsPrincipal userContext);
    }
}
