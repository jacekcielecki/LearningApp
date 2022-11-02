﻿
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Interfaces
{
    public interface IQuestionService
    {
        int? Create(CreateQuestionRequest questionRequest, int categoryId);
        IEnumerable<QuestionDto> GetAllByCategory(int categoryId);
        IEnumerable<QuestionDto> GetLesson(int categoryId, int level);
        QuestionDto Update(int id, UpdateQuestionRequest updateQuestionRequest);
        void Delete(int id);
    }
}