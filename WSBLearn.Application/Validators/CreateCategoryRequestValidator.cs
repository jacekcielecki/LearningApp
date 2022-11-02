using FluentValidation;
using WSBLearn.Application.Requests;

namespace WSBLearn.Application.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(40);            
            
            RuleFor(r => r.Description)
                .NotEmpty()
                .MaximumLength(140);            
            
            RuleFor(r => r.IconUrl)
                .NotEmpty()
                .MaximumLength(140);

            RuleFor(r => r.QuestionsPerLesson)
                .NotNull()
                .NotEmpty();

            RuleFor(r => r.LessonsPerLevel)
                .NotNull()
                .NotEmpty();
        }
    }
}
