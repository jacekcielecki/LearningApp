using AutoMapper;
using FluentValidation;
using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Achievement;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Application.Services
{

    public class AchievementService : IAchievementService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAchievementRequest> _createAchievementRequest;
        private readonly IValidator<UpdateAchievementRequest> _updateAchievementRequest;

        public AchievementService(WsbLearnDbContext dbContext, IMapper mapper, IValidator<CreateAchievementRequest> createAchievementRequest, IValidator<UpdateAchievementRequest> updateAchievementRequest)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createAchievementRequest = createAchievementRequest;
            _updateAchievementRequest = updateAchievementRequest;
        }

        public async Task<List<AchievementDto>> GetAllAsync()
        {
            var entities = await _dbContext.Achievements.ToListAsync();

            return _mapper.Map<List<AchievementDto>>(entities);
        }

        public async Task<AchievementDto> CreateAsync(CreateAchievementRequest request)
        {
            var validationResult = await _createAchievementRequest.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var entity = _mapper.Map<Achievement>(request);
            _dbContext.Achievements.Add(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AchievementDto>(entity);
        }

        public async Task<AchievementDto> UpdateAsync(int id, UpdateAchievementRequest request)
        {
            var validationResult = await _updateAchievementRequest.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var entity = await _dbContext.Achievements.FindAsync(id);
            if (entity is null)
                throw new NotFoundException(nameof(Achievement));

            entity.Name = request.Name;
            entity.Description = request.Description;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AchievementDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Achievements.FindAsync(id);
            if (entity is null)
                throw new NotFoundException(nameof(Achievement));

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
