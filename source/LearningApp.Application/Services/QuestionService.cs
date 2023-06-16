using AutoMapper;
using FluentValidation;
using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Question;
using LearningApp.Domain.Common;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateQuestionRequest> _createQuestionRequestValidator;
        private readonly IValidator<UpdateQuestionRequest> _updateQuestionRequestValidator;

        public QuestionService(LearningAppDbContext dbContext, IMapper mapper,
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
            var entities = await _dbContext
                .Questions
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
            if (entities is null)
                throw new NotFoundException(nameof(Question));

            return _mapper.Map<List<QuestionDto>>(entities);
        }

        public async Task<List<QuestionDto>> GetAllByLevelAsync(int categoryId, int level)
        {
            var entities = await _dbContext
                .Questions
                .Where(r => r.CategoryId == categoryId)
                .Where(r => r.Level == level)
                .ToListAsync();

            return _mapper.Map<List<QuestionDto>>(entities);
        }

        public async Task<List<QuestionDto>> GetQuizAsync(int categoryId, int level, int userId)
        {
            var category = await _dbContext.Categories
                .Include(r => r.Questions)
                .FirstOrDefaultAsync(r => r.Id == categoryId);
            if (category is null)
                throw new NotFoundException(nameof(Category));
            if (level is < 0 or > 3)
                throw new ArgumentException(Messages.InvalidLevel);

            var user = await _dbContext.Users.Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException(nameof(User));

            var userCategoryProgress = user.UserProgress.CategoryProgress
                .FirstOrDefault(u => u.CategoryId == categoryId);
            if (userCategoryProgress?.LevelProgresses is null)
                await CreateUserCategoryProgress(user, category);

            var questions = await GetRandomQuestions(categoryId, level);
            return questions;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionRequest request, int categoryId)
        {
            var category = await _dbContext
                .Categories
                .FindAsync(categoryId);
            if (category is null)
                throw new NotFoundException(nameof(Category));
            var validationResult = await _createQuestionRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            var entity = _mapper.Map<Question>(request);
            entity.Category = category;
            entity.DateCreated = DateTime.Now;
            await _dbContext.Questions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<QuestionDto>(entity);
        }

        public async Task<QuestionDto> UpdateAsync(int id, UpdateQuestionRequest request)
        {
            var entity = await _dbContext
                .Questions
                .FindAsync(id);
            if (entity is null)
                throw new NotFoundException(nameof(Question));
            var category = await _dbContext.Categories.FindAsync(request.CategoryId);
            if (category is null)
                throw new NotFoundException(nameof(Category));
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
                throw new NotFoundException(nameof(Question));

            _dbContext.Questions.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<List<QuestionDto>> GetRandomQuestions(int categoryId, int level)
        {
            var category = await _dbContext
                .Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            var questions = await _dbContext
                .Questions
                .Where(x => x.CategoryId == categoryId 
                            && x.Level == level)
                .ToListAsync();

            var selectedQuestions = new List<Question>();
            var random = new Random();

            while (selectedQuestions.Count < category?.QuestionsPerQuiz)
            {
                if (!questions.Any())
                    break;

                var randomQuestionId = random.Next(0, questions.Count - 1);
                selectedQuestions.Add(questions[randomQuestionId]);
                questions.RemoveAt(randomQuestionId);
            }

            return _mapper.Map<List<QuestionDto>>(selectedQuestions);
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
                new()
                {
                    LevelName = "Easy",
                    FinishedQuiz = 0,
                    QuizToFinish = category.QuizPerLevel,
                    LevelCompleted = false,
                    CategoryProgressId = categoryProgress.Id,
                    CategoryProgress = categoryProgress
                },
                new()
                {
                    LevelName = "Medium",
                    FinishedQuiz = 0,
                    QuizToFinish = category.QuizPerLevel,
                    LevelCompleted = false,
                    CategoryProgressId = categoryProgress.Id,
                    CategoryProgress = categoryProgress
                },
                new()
                {
                    LevelName = "Hard",
                    FinishedQuiz = 0, 
                    QuizToFinish = category.QuizPerLevel,
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
