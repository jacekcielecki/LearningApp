using FluentValidation;
using LearningApp.Application.Extensions;

namespace LearningApp.Application.Requests.Category
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
