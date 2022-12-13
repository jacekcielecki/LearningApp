using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Question;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateQuestionRequest> _createQuestionRequestValidator;
        private readonly IValidator<UpdateQuestionRequest> _updateQuestionRequestValidator;

        public QuestionService(WsbLearnDbContext dbContext, IMapper mapper, 
            IValidator<CreateQuestionRequest> createQuestionRequestValidator, 
            IValidator<UpdateQuestionRequest> updateQuestionRequestValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createQuestionRequestValidator = createQuestionRequestValidator;
            _updateQuestionRequestValidator = updateQuestionRequestValidator;
        }

        public async Task<List<QuestionDto>> GetAllByCategoryAsync(int categoryId)
        {
            var entities = await _dbContext.Questions
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
            if (entities is null)
                throw new NotFoundException("Category not found!");

            return _mapper.Map<List<QuestionDto>>(entities);
        }

        public async Task<List<QuestionDto>> GetQuizAsync(int categoryId, int level, int userId)
        {
            var category = await _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefaultAsync(r => r.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found!");
            if (level is < 0 or > 3)
                throw new ArgumentException("Given level is invalid");

            var user = await _dbContext.Users.Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found!");

            var userCategoryProgress = user.UserProgress.CategoryProgress
                .FirstOrDefault(u => u.CategoryId == categoryId);
            if (userCategoryProgress?.LevelProgresses is null)
                await CreateUserCategoryProgress(user, category);

            var questions = category.Questions
                .Where(r => r.Level == level).ToList();
            var selectedQuestions = new List<Question>();
            var random = new Random();
            while (selectedQuestions.Count() < category.QuestionsPerLesson)
            {
                if (!questions.Any())
                    break;

                var randomQuestionId = random.Next(0, questions.Count() - 1);
                selectedQuestions.Add(questions[randomQuestionId]);
                questions.RemoveAt(randomQuestionId);
            }

            return _mapper.Map<List<QuestionDto>>(selectedQuestions);
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionRequest request, int categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));
            var validationResult = await _createQuestionRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            var entity = _mapper.Map<Question>(request);
            entity.Category = category;
            await _dbContext.Questions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<QuestionDto>(entity);
        }

        public async Task<QuestionDto> UpdateAsync(int id, UpdateQuestionRequest request)
        {
            var entity = await _dbContext.Questions.FindAsync(id);
            if (entity is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Question"));
            var category = await _dbContext.Categories.FindAsync(request.CategoryId);
            if (category is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Category"));
            var validationResult = await _updateQuestionRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            entity.QuestionContent = request.QuestionContent;
            entity.ImageUrl = request.ImageUrl;
            entity.A = request.A;
            entity.B = request.B;
            entity.C = request.C;
            entity.D = request.D;
            entity.CorrectAnswer = request.CorrectAnswer;
            entity.Level = request.Level;
            entity.CategoryId = request.CategoryId;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<QuestionDto>(entity); ;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Questions.FindAsync(id);
            if (entity is null)
                throw new NotFoundException(string.Format(Messages.InvalidId, "Question"));

            _dbContext.Questions.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateUserCategoryProgress(User user, Category category)
        {
            var categoryProgress = new CategoryProgress
            {
                CategoryName = category.Name,
                CategoryId = category.Id,
                UserProgressId = user.UserProgressId
            };
            await _dbContext.CategoryProgresses.AddAsync(categoryProgress); 
            await _dbContext.SaveChangesAsync();

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

            await _dbContext.LevelProgresses.AddRangeAsync(defaultLevelProgressesList);
            await _dbContext.SaveChangesAsync();
        }
    }
}
