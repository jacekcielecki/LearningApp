
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests;

namespace WSBLearn.Application.Interfaces
{
    public interface IQuestionService
    {
        int? Create(CreateQuestionRequest questionRequest);
        IEnumerable<QuestionDto>? GetAll();
        QuestionDto GetById(int id);
        QuestionDto Update(int id, UpdateQuestionRequest updateQuestionRequest);
        void Delete(int id);
    }
}
