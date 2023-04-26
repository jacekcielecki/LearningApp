using FluentValidation;
using LearningApp.Application.Extensions;
using LearningApp.Domain.Common;

namespace LearningApp.Application.Requests.Question
{
    public class UpdateQuestionRequestValidator : AbstractValidator<UpdateQuestionRequest>
    {
        public UpdateQuestionRequestValidator()
        {
            RuleFor(r => r.QuestionContent)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(r => r.ImageUrl)
                .MaximumLength(1000)
                .NotEmpty();

            RuleFor(r => r.A)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(r => r.B)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(r => r.C)
                .MaximumLength(1000)
                .NotNull()
                .Unless(r => r.CorrectAnswer.ToString().ToLower() != "c")
                .WithMessage(ValidationMessages.AnswerRequired('c'))
                .NotEmpty();

            RuleFor(r => r.D)
                .MaximumLength(1000)
                .NotNull()
                .Unless(r => r.CorrectAnswer.ToString().ToLower() != "d")
                .WithMessage(ValidationMessages.AnswerRequired('d'))
                .NotEmpty();

            RuleFor(r => r.CorrectAnswer)
                .NotNull()
                .NotEmpty()
                .Custom((value, context) =>
                {
                    string[] validCorrectAnswers = { "a", "b", "c", "d" };
                    if (!validCorrectAnswers.Contains(value.ToString().ToLower()))
                    {
                        context.AddFailure("CorrectAnswer", ValidationMessages.CorrectAnswerOutOfRange);
                    }
                });

            RuleFor(r => r.Level)
                .NotNull()
                .NotEmpty()
                .Custom((value, context) =>
                {
                    int[] validLevels = { 1, 2, 3 };
                    if (!validLevels.Contains(value))
                    {
                        context.AddFailure("Level", ValidationMessages.LevelOutOfRange);
                    }
                });

            RuleFor(r => r.ImageUrl)
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
