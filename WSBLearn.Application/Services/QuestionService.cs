using AutoMapper;
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

        public QuestionService(WsbLearnDbContext dbContext, ILogger<QuestionService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public int? Create(CreateQuestionRequest questionRequest)
        {
            Question question = _mapper.Map<Question>(questionRequest);
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();

            return question.Id;
        }

        public IEnumerable<QuestionDto>? GetAll()
        {
            IEnumerable<Question> questions = _dbContext.Questions.AsEnumerable();
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            return questionDtos;
        }

        public QuestionDto GetById(int id)
        {
            Question? question = _dbContext.Questions.FirstOrDefault(q => q.Id == id);
            if (question is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Question"));

            var questionDto = _mapper.Map<QuestionDto>(question);

            return questionDto;
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

            question.QuestionContent = updateQuestionRequest.QuestionContent;
            question.ImageUrl = updateQuestionRequest.ImageUrl;
            question.A = updateQuestionRequest.A;
            question.B = updateQuestionRequest.B;
            question.C = updateQuestionRequest.C;
            question.D = updateQuestionRequest.D;
            question.CorrectAnswer = updateQuestionRequest.CorrectAnswer;
            question.Level = updateQuestionRequest.Level;
            question.CategoryId = updateQuestionRequest.CategoryId;

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
