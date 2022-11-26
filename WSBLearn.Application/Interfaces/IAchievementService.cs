using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.Achievement;

namespace WSBLearn.Application.Interfaces
{
    public interface IAchievementService
    {
        AchievementDto Create(CreateAchievementRequest request);
        IEnumerable<AchievementDto> GetAll();
        AchievementDto Update(int id, UpdateAchievementRequest request);
        void Delete(int id);
    }
}
