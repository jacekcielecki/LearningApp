using FluentValidation;

namespace LearningApp.Application.Requests.User
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {

            RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(40);

            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password);

            RuleFor(r => r.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
