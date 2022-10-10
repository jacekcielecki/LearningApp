using Microsoft.AspNetCore.Identity;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public class UserService : IUserService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;


        public UserService(WsbLearnDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Register(CreateUserRequest createUserRequest)
        {
            var user = new User()
            {
                Username = createUserRequest.Username,
                EmailAddress = createUserRequest.EmailAddress,
                RoleId = createUserRequest.RoleId,
            };
            user.Password = _passwordHasher.HashPassword(user, createUserRequest.Password);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public string Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
