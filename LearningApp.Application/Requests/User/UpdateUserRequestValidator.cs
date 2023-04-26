using FluentValidation;
using LearningApp.Domain.Common;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Requests.User
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
                        context.AddFailure("Username", ValidationMessages.UsernameTaken(value));
                    }
                });

            RuleFor(r => r.EmailAddress)
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var isEmailAddressNotUnique = _dbContext.Users.Any(u => u.EmailAddress == value);
                    if (isEmailAddressNotUnique)
                    {
                        context.AddFailure("Email Address", ValidationMessages.EmailAddressTaken(value));
                    }
                });

            RuleFor(r => r.ProfilePictureUrl)
                .MaximumLength(400);
        }
    }
}
