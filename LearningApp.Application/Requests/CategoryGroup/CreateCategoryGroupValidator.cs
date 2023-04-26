using FluentValidation;
using LearningApp.Application.Extensions;
using LearningApp.Domain.Common;

namespace LearningApp.Application.Requests.CategoryGroup
{
    public class CreateCategoryGroupValidator : AbstractValidator<CreateCategoryGroupRequest>
    {
        public CreateCategoryGroupValidator()
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
                        context.AddFailure("IconUrl", ValidationMessages.InvalidUrl);
                    }
                });
        }
    }
}
