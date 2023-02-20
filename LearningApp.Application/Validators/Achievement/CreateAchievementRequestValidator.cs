using FluentValidation;
using LearningApp.Application.Requests.Achievement;

namespace LearningApp.Application.Validators.Achievement
{
    public class CreateAchievementRequestValidator : AbstractValidator<CreateAchievementRequest>
    {
        public CreateAchievementRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);

            RuleFor(r => r.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);
        }
    }
}
