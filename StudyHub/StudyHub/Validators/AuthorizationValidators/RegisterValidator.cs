using FluentValidation;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Utility;

namespace StudyHub.Validators.AuthorizationValidators;

public class RegisterValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Your email is invalid");

        RuleFor(dto => dto.FullName)
            .NotEmpty()
            .Matches(ValidationRegexes.FullNameRegexes)
            .WithMessage("'{PropertyName}' should only contain letters.");

        RuleFor(dto => dto.Password)
           .NotEmpty()
           .MinimumLength(8)
           .WithMessage("Your password must be 8 symbols at least");

        RuleFor(dto => dto.Token)
            .NotEmpty();
    }
}
