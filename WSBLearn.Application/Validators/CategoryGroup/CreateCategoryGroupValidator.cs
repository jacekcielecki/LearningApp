using FluentValidation;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Requests.CategoryGroup;

namespace WSBLearn.Application.Validators.CategoryGroup
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
                        context.AddFailure("IconUrl", "Field is not empty and not a valid fully-qualified http, https or ftp URL");
                    }
                });
        }
    }
}
