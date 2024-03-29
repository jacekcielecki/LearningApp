﻿using AutoMapper;
using FluentValidation;
using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.User;
using LearningApp.Application.Settings;
using LearningApp.Domain.Common;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearningApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserRequestValidator;
        private readonly IValidator<UpdateUserPasswordRequest> _updateUserPasswordRequestValidator;
        private readonly IValidator<ResetPasswordRequest> _resetPasswordRequestValidator;
        private readonly JwtAuthenticationSettings _authenticationSettings;
        private readonly AzureBlobStorageSettings _blobStorageSettings;
        private readonly IMapper _mapper;

        public UserService(LearningAppDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            IValidator<CreateUserRequest> createUserRequestValidator,
            IValidator<UpdateUserRequest> updateUserRequestValidator,
            IValidator<UpdateUserPasswordRequest> updateUserPasswordRequestValidator,
            IValidator<ResetPasswordRequest> resetPasswordRequestValidator,
        JwtAuthenticationSettings authenticationSettings,
            AzureBlobStorageSettings blobStorageSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _createUserRequestValidator = createUserRequestValidator;
            _updateUserRequestValidator = updateUserRequestValidator;
            _updateUserPasswordRequestValidator = updateUserPasswordRequestValidator;
            _resetPasswordRequestValidator = resetPasswordRequestValidator;
            _authenticationSettings = authenticationSettings;
            _blobStorageSettings = blobStorageSettings;
            _mapper = mapper;
        }

        public async Task VerifyAccount(string verificationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.VerificationToken == verificationToken);
            if (user is null) throw new InvalidVerificationTokenException("Invalid verification token");

            var isTokenExpired = user.VerificationTokenExpireDate < DateTime.Now;
            if (isTokenExpired) throw new InvalidVerificationTokenException("Verification token has expired");

            user.IsVerified = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> RegisterAsync(CreateUserRequest request)
        {
            var validationResult = await _createUserRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            var user = new User
            {
                Username = request.Username,
                EmailAddress = request.EmailAddress,
                RoleId = 2,
                ProfilePictureUrl = string.IsNullOrEmpty(request.ProfilePictureUrl) ? _blobStorageSettings.DefaultProfilePictureUrl : request.ProfilePictureUrl,
                IsVerified = false,
                VerificationToken = GenerateVerificationToken(),
                VerificationTokenExpireDate = DateTime.Now.AddDays(1)
            };
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var userProgress = new UserProgress
            {
                ExperiencePoints = 0,
                Level = 1,
                TotalCompletedQuiz = 0,
                UserId = user.Id
            };
            _dbContext.UserProgresses.Add(userProgress);
            await _dbContext.SaveChangesAsync();

            user.UserProgressId = userProgress.Id;
            await _dbContext.SaveChangesAsync();

            return user.EmailAddress;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users
                .Include(e => e.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmailAddress == loginDto.Email || e.Username == loginDto.Email);
            if (user is null) throw new BadHttpRequestException(Messages.AuthorizationFailed);

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed) throw new BadHttpRequestException(Messages.AuthorizationFailed);

            return GenerateJwtToken(user);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var entities = await _dbContext .Users
                .Include(u => u.Role)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.Achievements)
                .Include(u => u.UserProgress)
                .ThenInclude(u => u.CategoryProgress)
                .ThenInclude(u => u.LevelProgresses)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Users
                .Include(e => e.Role)
                .Include(e => e.UserProgress)
                .ThenInclude(e => e.Achievements)
                .Include(e => e.UserProgress)
                .ThenInclude(e => e.CategoryProgress)
                .ThenInclude(e => e.LevelProgresses)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) throw new NotFoundException(nameof(User));

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<List<UserRankingDto>> GetSortByExpAsync()
        {
            var entities = await _dbContext.Users
                .Include(u => u.UserProgress)
                .OrderByDescending(r => r.UserProgress.ExperiencePoints)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<UserRankingDto>>(entities);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserRequest request)
        {
            var entity = await _dbContext.Users
                .FindAsync(id);
            if (entity is null) throw new NotFoundException(nameof(User));
            if (entity.Id == 1 && entity.EmailAddress == "root") throw new ResourceProtectedException();

            var validationResult = await _updateUserRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            if (!string.IsNullOrEmpty(request.Username))
                entity.Username = request.Username;
            if (!string.IsNullOrEmpty(request.EmailAddress))
                entity.EmailAddress = request.EmailAddress;
            if (!string.IsNullOrEmpty(request.ProfilePictureUrl))
                entity.ProfilePictureUrl = request.ProfilePictureUrl;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> UpdateUserRoleAsync(int id, int roleId)
        {
            var entity = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (entity is null) throw new NotFoundException(nameof(User));
            if (entity.Id == 1 && entity.Username == "root") throw new ResourceProtectedException();

            var role = _dbContext.Roles
                .FirstOrDefault(r => r.Id == roleId);
            if (role is null) throw new NotFoundException(nameof(Role));

            entity.RoleId = roleId;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task UpdateUserPasswordAsync(int id, UpdateUserPasswordRequest request)
        {
            var entity = await _dbContext.Users
                .FindAsync(id);
            if (entity is null) throw new NotFoundException(nameof(User));
            if (entity.Id == 1 && entity.Username == "root") throw new ResourceProtectedException();

            var passwordVerification = _passwordHasher.VerifyHashedPassword(entity, entity.Password, request.OldPassword);
            if (passwordVerification == PasswordVerificationResult.Failed) throw new BadHttpRequestException(Messages.InvalidPassword);

            var validationResult = await _updateUserPasswordRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            entity.Password = _passwordHasher.HashPassword(entity, request.NewPassword);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Users
                .FindAsync(id);
            if (entity is null) throw new NotFoundException(nameof(User));
            if (entity.Id == 1 && entity.Username == "root") throw new ResourceProtectedException();

            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetUserVerificationToken(string userEmail)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.EmailAddress == userEmail);

            if (user is null) throw new NotFoundException(nameof(User));

            var isTokenExpired = user.VerificationTokenExpireDate < DateTime.Now;
            if (isTokenExpired)
            {
                user.VerificationToken = GenerateVerificationToken();
                user.VerificationTokenExpireDate = DateTime.Now.AddDays(1);
                await _dbContext.SaveChangesAsync();
            }

            return user.VerificationToken;
        }

        public async Task<string> GetPasswordResetToken(string userEmail)
        {
            var user = await _dbContext.Users
                .Where(x => x.EmailAddress == userEmail)
                .FirstOrDefaultAsync();

            if (user is null) throw new NotFoundException(nameof(User));

            user.ResetPasswordToken = GenerateVerificationToken();
            user.ResetPasswordTokenExpireDate = DateTime.Now.AddDays(1);
            await _dbContext.SaveChangesAsync();

            return user.ResetPasswordToken;
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var validationResult = await _resetPasswordRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors[0].ToString());

            var user = await _dbContext.Users
                .Where(x => x.ResetPasswordToken == request.Token)
                .FirstOrDefaultAsync();

            if (user is null) throw new NotFoundException(nameof(User));

            var isTokenExpired = user.ResetPasswordTokenExpireDate < DateTime.Now;
            if (isTokenExpired) throw new InvalidVerificationTokenException("Reset password token has expired");

            user.Password = _passwordHasher.HashPassword(user, request.Password);
            await _dbContext.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
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

        private string GenerateVerificationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
