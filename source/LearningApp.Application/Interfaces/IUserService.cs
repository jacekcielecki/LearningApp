using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.User;

namespace LearningApp.Application.Interfaces
{
    public interface IUserService
    {
        Task VerifyAccount(string verificationToken);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<string> RegisterAsync(CreateUserRequest request);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<List<UserRankingDto>> GetSortByExpAsync();
        Task<UserDto> UpdateAsync(int id, UpdateUserRequest request);
        Task<UserDto> UpdateUserRoleAsync(int id, int roleId);
        Task UpdateUserPasswordAsync(int id, UpdateUserPasswordRequest request);
        Task DeleteAsync(int id);
        Task<string> GetUserVerificationToken(string userEmail);
        Task<string> GetPasswordResetToken(string userEmail);
        Task ResetPassword(ResetPasswordRequest request);
    }
}
