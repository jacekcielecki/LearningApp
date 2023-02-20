using FluentValidation;

namespace LearningApp.Application.Requests.Achievement
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
