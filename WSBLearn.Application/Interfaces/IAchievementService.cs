using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.Achievement;

namespace WSBLearn.Application.Interfaces
{
    public interface IAchievementService
    {
        Task<List<AchievementDto>> GetAllAsync();
        Task<AchievementDto> CreateAsync(CreateAchievementRequest request);
        Task<AchievementDto> UpdateAsync(int id, UpdateAchievementRequest request);
        Task DeleteAsync(int id);
    }
}
