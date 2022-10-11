using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using FluentValidation.Results;
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
        private IValidator<CreateUserRequest> _createUserRequestValidator;


        public UserService(WsbLearnDbContext dbContext, IPasswordHasher<User> passwordHasher, 
            IValidator<CreateUserRequest> createUserRequestValidator)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _createUserRequestValidator = createUserRequestValidator;
        }

        public void Register(CreateUserRequest createUserRequest)
        {
            ValidationResult result = _createUserRequestValidator.Validate(createUserRequest);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors[0].ToString());
            }

            var user = new User()
            {
                Username = createUserRequest.Username,
                EmailAddress = createUserRequest.EmailAddress,
                RoleId = 2,
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
