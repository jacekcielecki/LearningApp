using AutoMapper;
using FluentValidation;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Achievement;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
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

        public AchievementDto Create(CreateAchievementRequest request)
        {
            var validationResult = _createAchievementRequest.Validate(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var entity = _mapper.Map<Achievement>(request);
            _dbContext.Achievements.Add(entity);
            _dbContext.SaveChanges();
            var dto = _mapper.Map<AchievementDto>(entity);

            return dto;
        }

        public IEnumerable<AchievementDto> GetAll()
        {
            var entities = _dbContext.Achievements.AsEnumerable();
            return _mapper.Map<IEnumerable<AchievementDto>>(entities);
        }

        public AchievementDto Update(int id, UpdateAchievementRequest request)
        {
            var validationResult = _updateAchievementRequest.Validate(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var entity = _dbContext.Achievements.FirstOrDefault(a => a.Id == id);
            if (entity is null)
                throw new NotFoundException("Achievement with given id not found");

            entity.Name = request.Name;
            entity.Description = request.Description;
            _dbContext.SaveChanges();

            return _mapper.Map<AchievementDto>(entity);
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Achievements.FirstOrDefault(a => a.Id == id);
            if (entity is null)
                throw new NotFoundException("Achievement with given id not found");

            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
