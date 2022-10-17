using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly ILogger<QuestionService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateQuestionRequest> _createQuestionRequestValidator;
        private readonly IValidator<UpdateQuestionRequest> _updateQuestionRequestValidator;

        public QuestionService(WsbLearnDbContext dbContext, ILogger<QuestionService> logger, IMapper mapper, 
            IValidator<CreateQuestionRequest> createQuestionRequestValidator, IValidator<UpdateQuestionRequest> updateQuestionRequestValidator)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _createQuestionRequestValidator = createQuestionRequestValidator;
            _updateQuestionRequestValidator = updateQuestionRequestValidator;
        }

        public int? Create(CreateQuestionRequest createQuestionRequest, int categoryId)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));

            ValidationResult validationResult = _createQuestionRequestValidator.Validate(createQuestionRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

            Question question = _mapper.Map<Question>(createQuestionRequest);
            question.Category = category;

            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();

            return question.Id;
        }

        public IEnumerable<QuestionDto> GetAllByCategory(int categoryId)
        {
            var category = _dbContext.Categories.Include(r => r.Questions).FirstOrDefault(r => r.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found!");

            var questions = category.Questions.AsEnumerable();
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            return (questionDtos);
        }

        public IEnumerable<QuestionDto> GetQuiz(int categoryId, int level)
        {
            var category = _dbContext.Categories.Include(r => r.Questions).FirstOrDefault(r => r.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found!");
            if ((level < 0) || (level > 3))
                throw new ArgumentException("Given level is invalid");

            var questions = category.Questions.Where(r => r.Level == level).AsEnumerable();
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            return (questionDtos);
        }

        public QuestionDto Update(int id, UpdateQuestionRequest updateQuestionRequest)
        {
            Question? question = _dbContext.Questions.FirstOrDefault(q => q.Id == id);
            if (question is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Question"));

            Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == updateQuestionRequest.CategoryId);
            if (category is null)
            {
                _logger.LogError(string.Format(Messages.InvalidId, "Category"));
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));
            }

            ValidationResult validationResult = _updateQuestionRequestValidator.Validate(updateQuestionRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

            question.QuestionContent = updateQuestionRequest.QuestionContent;
            question.ImageUrl = updateQuestionRequest.ImageUrl;
            question.A = updateQuestionRequest.A;
            question.B = updateQuestionRequest.B;
            question.C = updateQuestionRequest.C;
            question.D = updateQuestionRequest.D;
            question.CorrectAnswer = updateQuestionRequest.CorrectAnswer;
            question.Level = updateQuestionRequest.Level;
            question.Category = category;

            var questionDto = _mapper.Map<QuestionDto>(question);
            _dbContext.SaveChanges();

            return questionDto;
        }

        public void Delete(int id)
        {
            Question? question = _dbContext.Questions.FirstOrDefault(q => q.Id == id);
            if (question is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Question"));

            _dbContext.Questions.Remove(question);
            _dbContext.SaveChanges();

            return;
        }
    }
}
