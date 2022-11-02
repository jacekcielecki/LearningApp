using FluentValidation;
using WSBLearn.Application.Requests;
using WSBLearn.Dal.Persistence;

namespace WSBLearn.Application.Validators
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

            RuleFor(r => r.RoleId)
                .Custom((value, context) =>
                {
                    var givenRoleExists = _dbContext.Roles.Any(r => r.Id == value);
                    if (!givenRoleExists)
                    {
                        context.AddFailure("Role Id", "Given role does not exist!");
                    }
                });
        }
    }
}
