using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.User;
using WSBLearn.Application.Responses;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserService
    {
        string Login(LoginDto loginDto);
        void Register(CreateUserRequest createUserRequest);
        IEnumerable<UserDto> GetAll();
        IEnumerable<UserRankingResponse> GetSortByExp();
        void Delete(int id);
        UserDto GetById(int id);
        UserDto Update(int id, UpdateUserRequest updateUserRequest);
        UserDto UpdateUserRole(int id, int roleId);
        void UpdateUserPassword(int id, UpdateUserPasswordRequest updateUserPasswordRequest);
    }
}
