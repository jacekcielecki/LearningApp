using FluentValidation;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Requests.Category;

namespace WSBLearn.Application.Validators.Category
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
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

            RuleFor(r => r.IconUrl)
                .Custom((value, context) =>
                {
                    var isUrlOrEmpty = value!.UrlOrEmpty();
                    if (!isUrlOrEmpty)
                    {
                        context.AddFailure("IconUrl", "Field is not empty and not a valid fully-qualified http, https or ftp URL");
                    }
                });
        }
    }
}
