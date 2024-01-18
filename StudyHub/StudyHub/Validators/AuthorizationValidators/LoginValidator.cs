using FluentValidation;
using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.Validators.AuthorizationValidators;

public class LoginValidator : AbstractValidator<LoginUserDTO>
{
    public LoginValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Your email is not real");

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Your password must be 8 symbols at least");
    }
}
