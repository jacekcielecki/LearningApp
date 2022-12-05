using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Question;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

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

            var validationResult = _createQuestionRequestValidator.Validate(createQuestionRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

            var question = _mapper.Map<Question>(createQuestionRequest);
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

        public IEnumerable<QuestionDto> GetQuiz(int categoryId, int level, int userId)
        {
            var category = _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefault(r => r.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found!");
            if ((level < 0) || (level > 3))
                throw new ArgumentException("Given level is invalid");

            var user = _dbContext.Users.Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefault(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found!");

            var userCategoryProgress = user.UserProgress.CategoryProgress
                .FirstOrDefault(u => u.CategoryId == categoryId);
            if (userCategoryProgress is null || userCategoryProgress.LevelProgresses is null)
                CreateUserCategoryProgress(user, category);

            var questions = category.Questions
                .Where(r => r.Level == level).ToList();
            var selectedQuestions = new List<Question>();
            var random = new Random();
            while (selectedQuestions.Count() < category.QuestionsPerLesson)
            {
                if (!questions.Any())
                    break;

                int randomQuestionId = random.Next(0, questions.Count() - 1);
                selectedQuestions.Add(questions[randomQuestionId]);
                questions.RemoveAt(randomQuestionId);
            }

            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(selectedQuestions);
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
        }

        private void CreateUserCategoryProgress(User user, Category category)
        {
            var categoryProgress = new CategoryProgress
            {
                CategoryName = category.Name,
                CategoryId = category.Id,
                UserProgressId = user.UserProgressId
            };
            _dbContext.CategoryProgresses.Add(categoryProgress);
            _dbContext.SaveChanges();

            var defaultLevelProgressesList = new List<LevelProgress>
            {
                new LevelProgress
                {
                    LevelName = "Easy",
                    FinishedQuizzes = 0,
                    QuizzesToFinish = category.LessonsPerLevel,
                    LevelCompleted = false,
                    CategoryProgressId = categoryProgress.Id,
                    CategoryProgress = categoryProgress
                },
                new LevelProgress
                {
                    LevelName = "Medium",
                    FinishedQuizzes = 0,
                    QuizzesToFinish = category.LessonsPerLevel,
                    LevelCompleted = false,
                    CategoryProgressId = categoryProgress.Id,
                    CategoryProgress = categoryProgress
                },
                new LevelProgress
                {
                    LevelName = "Hard",
                    FinishedQuizzes = 0,
                    QuizzesToFinish = category.LessonsPerLevel,
                    LevelCompleted = false,
                    CategoryProgressId = categoryProgress.Id,
                    CategoryProgress = categoryProgress
                }
            };

            _dbContext.LevelProgresses.AddRange(defaultLevelProgressesList);
            _dbContext.SaveChanges();
        }
    }
}
