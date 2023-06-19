using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Question;
using System.Security.Claims;

namespace LearningApp.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionDto>> GetAllByCategoryAsync(int categoryId, ClaimsPrincipal userContext);
        Task<List<QuestionDto>> GetQuizAsync(int categoryId, int level, ClaimsPrincipal userContext);
        Task<QuestionDto> CreateAsync(CreateQuestionRequest request, int categoryId, ClaimsPrincipal userContext);
        Task<QuestionDto> UpdateAsync(int id, UpdateQuestionRequest request, ClaimsPrincipal userContext);
        Task DeleteAsync(int id, ClaimsPrincipal userContext);
        Task<List<QuestionDto>> GetAllByLevelAsync(int categoryId, int level, ClaimsPrincipal userContext);
    }
}
