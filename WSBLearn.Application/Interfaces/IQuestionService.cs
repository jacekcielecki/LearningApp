using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.Question;

namespace WSBLearn.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionDto>> GetAllByCategoryAsync(int categoryId);
        Task<List<QuestionDto>> GetQuizAsync(int categoryId, int level, int userId);
        Task<QuestionDto> CreateAsync(CreateQuestionRequest request, int categoryId);
        Task<QuestionDto> UpdateAsync(int id, UpdateQuestionRequest request);
        Task DeleteAsync(int id);
    }
}
