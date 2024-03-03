using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Question;

namespace LearningApp.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionDto>> GetAllByCategoryAsync(int categoryId);
        Task<List<QuestionDto>> GetQuizAsync(int categoryId, int level);
        Task<QuestionDto> CreateAsync(CreateQuestionRequest request, int categoryId);
        Task<QuestionDto> UpdateAsync(int id, UpdateQuestionRequest request);
        Task DeleteAsync(int id);
        Task<List<QuestionDto>> GetAllByLevelAsync(int categoryId, int level);
    }
}
