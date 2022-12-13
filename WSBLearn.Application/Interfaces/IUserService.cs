using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.User;
using WSBLearn.Application.Responses;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(CreateUserRequest request);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<List<UserRankingResponse>> GetSortByExpAsync();
        Task<UserDto> UpdateAsync(int id, UpdateUserRequest request);
        Task<UserDto> UpdateUserRoleAsync(int id, int roleId);
        Task UpdateUserPasswordAsync(int id, UpdateUserPasswordRequest request);
        Task DeleteAsync(int id);
    }
}
