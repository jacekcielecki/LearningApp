using FluentValidation;
using WSBLearn.Application.Requests.User;
using WSBLearn.Dal.Persistence;

namespace WSBLearn.Application.Validators.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly WsbLearnDbContext _dbContext;

        public UpdateUserRequestValidator(WsbLearnDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(r => r.Username)
                .NotNull()
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

            RuleFor(r => r.Password)
                .NotNull()
                .MinimumLength(6)
                .MaximumLength(40);

            RuleFor(r => r.EmailAddress)
                .NotNull()
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
                .NotNull()
                .MaximumLength(60);
        }
    }
}
