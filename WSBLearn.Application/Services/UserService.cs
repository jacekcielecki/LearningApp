using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
        private readonly JwtAuthenticationSettings _authenticationSettings;

        public UserService(WsbLearnDbContext dbContext, IPasswordHasher<User> passwordHasher, 
            IValidator<CreateUserRequest> createUserRequestValidator,
            JwtAuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _createUserRequestValidator = createUserRequestValidator;
            _authenticationSettings = authenticationSettings;
        }

        public void Register(CreateUserRequest createUserRequest)
        {
            ValidationResult validationResult = _createUserRequestValidator.Validate(createUserRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ToString());
            }

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
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.EmailAddress == loginDto.Login || u.Username == loginDto.Login);

            if (user is null)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.ExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.Issuer,
                audience: _authenticationSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
