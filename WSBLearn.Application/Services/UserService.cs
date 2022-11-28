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
using WSBLearn.Dal.Persistence;
using WSBLearn.Domain.Entities;
using AutoMapper;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Exceptions;
using WSBLearn.Application.Requests.User;
using WSBLearn.Application.Responses;
using WSBLearn.Application.Settings;

namespace WSBLearn.Application.Services
{
    public class UserService : IUserService
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserRequestValidator;
        private readonly IValidator<UpdateUserPasswordRequest> _updateUserPasswordRequestValidator;
        private readonly JwtAuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;

        public UserService(WsbLearnDbContext dbContext, IPasswordHasher<User> passwordHasher, 
            IValidator<CreateUserRequest> createUserRequestValidator,
            IValidator<UpdateUserRequest> updateUserRequestValidator,
            IValidator<UpdateUserPasswordRequest> updateUserPasswordRequestValidator,
        JwtAuthenticationSettings authenticationSettings,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _createUserRequestValidator = createUserRequestValidator;
            _updateUserRequestValidator = updateUserRequestValidator;
            _updateUserPasswordRequestValidator = updateUserPasswordRequestValidator;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
        }

        public void Register(CreateUserRequest createUserRequest)
        {
            var validationResult = _createUserRequestValidator.Validate(createUserRequest);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());
            var user = new User()
            {
                Username = createUserRequest.Username,
                EmailAddress = createUserRequest.EmailAddress,
                RoleId = 2,
                ProfilePictureUrl = string.IsNullOrEmpty(createUserRequest.ProfilePictureUrl) ? Defaults.ProfilePictureUrl : createUserRequest.ProfilePictureUrl,
            };
            user.Password = _passwordHasher.HashPassword(user, createUserRequest.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var userProgress = new UserProgress
            {
                ExperiencePoints = 0,
                Level = 1,
                TotalCompletedQuiz = 0,
                UserId = user.Id
            };
            _dbContext.UserProgresses.Add(userProgress);
            _dbContext.SaveChanges();

            user.UserProgressId = userProgress.Id;
            _dbContext.SaveChanges();
        }

        public string Login(LoginDto loginDto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.EmailAddress == loginDto.Login || u.Username == loginDto.Login);
            if (user is null)
                throw new BadHttpRequestException("Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadHttpRequestException("Invalid username or password");

            return GenerateToken(user);
        }

        public IEnumerable<UserDto> GetAll()
        {
            IEnumerable<User> users = _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.Achievements)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .AsEnumerable();

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users); 
            return userDtos;
        }

        public UserDto GetById(int id)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.Achievements)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");
           
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public IEnumerable<UserRankingResponse> GetSortByExp()
        {
            var users = _dbContext.Users
                .Include(u => u.UserProgress).Skip(1)
                .OrderByDescending(r => r.UserProgress.ExperiencePoints)
                .AsEnumerable();

            var userRankingResponses = _mapper.Map<IEnumerable<UserRankingResponse>>(users);
            return userRankingResponses;
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");
            if (user.Id == 1 && user.Username == "root")
                throw new ResourceProtectedException("Action forbidden, resource is protected");

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public UserDto Update(int id, UpdateUserRequest updateUserRequest)
        {
            var user = _dbContext.Users.Include(u => u.UserProgress).FirstOrDefault();
            if (user is null)
                throw new NotFoundException("User with given id not found");
            if (user.Id == 1 && user.Username == "root")
                throw new ResourceProtectedException("Action forbidden, resource is protected");

            ValidationResult validationResult = _updateUserRequestValidator.Validate(updateUserRequest);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            if (!string.IsNullOrEmpty(updateUserRequest.Username))
                user.Username = updateUserRequest.Username;
            if (!string.IsNullOrEmpty(updateUserRequest.EmailAddress))
                user.EmailAddress = updateUserRequest.EmailAddress;
            if (!string.IsNullOrEmpty(updateUserRequest.ProfilePictureUrl))
                user.ProfilePictureUrl = updateUserRequest.ProfilePictureUrl;
            _dbContext.SaveChanges();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public UserDto UpdateUserRole(int id, int roleId)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");
            if (user.Id == 1 && user.Username == "root")
                throw new ResourceProtectedException("Action forbidden, resource is protected");
            var role = _dbContext.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role is null)
                throw new NotFoundException("Role with given id not found");

            user.RoleId = roleId;
            _dbContext.SaveChanges();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public void UpdateUserPassword(int id, UpdateUserPasswordRequest updateUserPasswordRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new NotFoundException("User with given id not found");
            if (user.Id == 1 && user.Username == "root")
                throw new ResourceProtectedException("Action forbidden, resource is protected");
            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, updateUserPasswordRequest.OldPassword);
            if (passwordVerification == PasswordVerificationResult.Failed)
                throw new BadHttpRequestException("Invalid Password");
            var validationResult = _updateUserPasswordRequestValidator.Validate(updateUserPasswordRequest);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors[0].ToString());

            user.Password = _passwordHasher.HashPassword(user, updateUserPasswordRequest.NewPassword);
            _dbContext.SaveChanges();

        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Audience = _authenticationSettings.Issuer,
                Issuer = _authenticationSettings.Issuer,
                Expires = DateTime.Now.AddDays(_authenticationSettings.ExpireDays),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
