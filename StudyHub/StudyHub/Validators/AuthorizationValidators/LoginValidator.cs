using FluentValidation;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Utility;

namespace StudyHub.Validators.AuthorizationValidators;

public class LoginValidator : AbstractValidator<LoginUserDTO>
{
    public LoginValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Your email is invalid");

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .Matches(ValidationRegexes.UpperCaseRegexes).WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(ValidationRegexes.LowerCaseRegexes).WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(ValidationRegexes.NumberRegexes).WithMessage("Your password must contain at least one number.")
            .Matches(ValidationRegexes.SymbolsRegexes).WithMessage("Your password must contain at least one (!? *.).");
    }
}
