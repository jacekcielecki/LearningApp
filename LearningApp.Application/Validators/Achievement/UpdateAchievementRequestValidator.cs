using FluentValidation;
using LearningApp.Application.Requests.Achievement;

namespace LearningApp.Application.Validators.Achievement
{
    public class UpdateAchievementRequestValidator : AbstractValidator<UpdateAchievementRequest>
    {
        public UpdateAchievementRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(40);

            RuleFor(r => r.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);
        }
    }
}
