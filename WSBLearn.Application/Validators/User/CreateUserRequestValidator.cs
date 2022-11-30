using FluentValidation;
using WSBLearn.Application.Requests.User;
using WSBLearn.Dal.Persistence;
using WSBLearn.Application.Extensions;

namespace WSBLearn.Application.Validators.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly WsbLearnDbContext _dbContext;

        public CreateUserRequestValidator(WsbLearnDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(r => r.Username)
                .NotNull()
                .NotEmpty()
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
                        context.AddFailure("Email Address", "Given Email Address is already in use");
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
                        context.AddFailure("ProfilePictureUrl", "Field is not empty and not a valid fully-qualified http, https or ftp URL");
                    }
                });
        }
    }
}
