using FluentValidation;
using LearningApp.Application.Extensions;
using LearningApp.Domain.Common;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Requests.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly LearningAppDbContext _dbContext;

        public CreateUserRequestValidator(LearningAppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(r => r.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(40);

            RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(40);

            RuleFor(r => r.EmailAddress)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var isEmailAddressNotUnique = _dbContext.Users.Any(u => u.EmailAddress == value);
                    if (isEmailAddressNotUnique)
                    {
                        context.AddFailure("Email Address", ValidationMessages.EmailAddressTaken(value));
                    }
                });

            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password);

            RuleFor(r => r.ProfilePictureUrl)
                .MaximumLength(400)
                .Custom((value, context) =>
                {
                    var isUrlOrEmpty = value!.UrlOrEmpty();
                    if (!isUrlOrEmpty)
                    {
                        context.AddFailure("ProfilePictureUrl", ValidationMessages.InvalidUrl);
                    }
                });
        }
    }
}
