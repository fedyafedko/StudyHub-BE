using FluentValidation;
using StudyHub.Common.Requests;
using StudyHub.Common.Utility;

namespace StudyHub.Validators.AuthorizationValidators;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Your email is invalid");

        RuleFor(request => request.NewPassword)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .Matches(ValidationRegexes.UpperCaseRegexes).WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(ValidationRegexes.LowerCaseRegexes).WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(ValidationRegexes.NumberRegexes).WithMessage("Your password must contain at least one number.")
            .Matches(ValidationRegexes.SymbolsRegexes).WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(request => request.ConfimPassword)
            .NotEmpty()
            .Equal(request => request.NewPassword)
            .WithMessage("Passwords are not equal");

        RuleFor(request => request.ResetToken)
            .NotEmpty();
    }
}
