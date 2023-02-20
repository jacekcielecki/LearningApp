using FluentValidation;
using LearningApp.Application.Extensions;
using LearningApp.Application.Requests.CategoryGroup;

namespace LearningApp.Application.Validators.CategoryGroup
{
    public class UpdateCategoryGroupValidator : AbstractValidator<UpdateCategoryGroupRequest>
    {
        public UpdateCategoryGroupValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(40);

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
