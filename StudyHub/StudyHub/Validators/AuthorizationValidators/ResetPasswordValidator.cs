using FluentValidation;
using StudyHub.Common.Requests;

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
            .NotEmpty();

        RuleFor(request => request.ConfimPassword)
            .NotEmpty()
            .Equal(request => request.NewPassword)
            .WithMessage("Passwords are not equal");

        RuleFor(request => request.ResetToken)
            .NotEmpty();
    }
}
