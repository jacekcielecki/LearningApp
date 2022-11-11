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
using AutoMapper;
using WSBLearn.Application.Exceptions;

namespace WSBLearn.Application.Services
{
    public class UserService : IUserService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
        private readonly JwtAuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;

        public UserService(WsbLearnDbContext dbContext, IPasswordHasher<User> passwordHasher, 
            IValidator<CreateUserRequest> createUserRequestValidator,
            JwtAuthenticationSettings authenticationSettings,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _createUserRequestValidator = createUserRequestValidator;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
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
                ProfilePictureUrl = createUserRequest.ProfilePictureUrl
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

        public IEnumerable<UserDto> GetAll()
        {
            IEnumerable<User> users = _dbContext.Users.Include(u => u.Role).AsEnumerable();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return userDtos;
        }

        public UserDto GetById(int id)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public UserDto Update(int id, UpdateUserRequest updateUserRequest)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");

            user.Username = updateUserRequest.Username;
            user.EmailAddress = updateUserRequest.EmailAddress;
            user.Password = _passwordHasher.HashPassword(user, updateUserRequest.Password);
            user.RoleId = updateUserRequest.RoleId;
            user.ProfilePictureUrl = updateUserRequest.ProfilePictureUrl;
            _dbContext.SaveChanges();
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
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
