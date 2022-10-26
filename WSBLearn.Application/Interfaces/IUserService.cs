using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests;

namespace WSBLearn.Application.Interfaces
{
    public interface IUserService
    {
        string Login(LoginDto loginDto);
        void Register(CreateUserRequest createUserRequest);
        IEnumerable<UserDto> GetAll();
        void Delete(int id);
        UserDto GetById(int id);
        UserDto Update(int id, UpdateUserRequest updateUserRequest);
    }
}
