using FluentValidation;
using StudyHub.Common.DTO.UserInvitation;

namespace StudyHub.Validators.UserValidator;

public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty();
        RuleFor(x => x.Telegram)
            .NotEmpty();
    }
}
