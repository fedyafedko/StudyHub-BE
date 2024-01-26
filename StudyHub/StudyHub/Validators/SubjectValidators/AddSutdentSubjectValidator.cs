using FluentValidation;
using StudyHub.Common.Requests;

namespace StudyHub.Validators.SubjectValidators;

public class AddSutdentSubjectValidator : AbstractValidator<AddStudentSubjectRequest>
{
    public AddSutdentSubjectValidator()
    {
        RuleFor(x => x.Emails)
            .NotEmpty()
            .ForEach(email => email.EmailAddress().WithMessage("Some of those emails is invalid"));
    }
}
