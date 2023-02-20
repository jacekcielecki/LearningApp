﻿using FluentValidation;
using LearningApp.Application.Requests.User;
using LearningApp.Dal.Persistence;

namespace LearningApp.Application.Validators.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly WsbLearnDbContext _dbContext;

        public UpdateUserRequestValidator(WsbLearnDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(r => r.Username)
                .MinimumLength(6)
                .MaximumLength(40)
                .Custom((value, context) =>
                {
                    var isUsernameNotUnique = _dbContext.Users.Any(u => u.Username == value);
                    if (isUsernameNotUnique)
                    {
                        context.AddFailure("Username", $"User {value} already exists");
                    }
                });

            RuleFor(r => r.EmailAddress)
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var isEmailAddressNotUnique = _dbContext.Users.Any(u => u.EmailAddress == value);
                    if (isEmailAddressNotUnique)
                    {
                        context.AddFailure("Email Address", "Given Email Address is already in use");
                    }
                });

            RuleFor(r => r.ProfilePictureUrl)
                .MaximumLength(400);
        }
    }
}
