using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Achievement;

namespace LearningApp.Application.Interfaces
{
    public interface IAchievementService
    {
        Task<List<AchievementDto>> GetAllAsync();
        Task<AchievementDto> CreateAsync(CreateAchievementRequest request);
        Task<AchievementDto> UpdateAsync(int id, UpdateAchievementRequest request);
        Task DeleteAsync(int id);
    }
}
