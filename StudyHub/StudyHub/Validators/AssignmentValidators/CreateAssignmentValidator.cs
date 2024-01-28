using FluentValidation;
using StudyHub.Common.DTO.Assignment;

namespace StudyHub.Validators.AssignmentValidators;

public class CreateAssignmentValidator : AbstractValidator<CreateAssignmentDTO>
{
    public CreateAssignmentValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.MaxMark)
            .GreaterThan(0);

        RuleFor(x => x.OpeningDate)
            .NotEmpty().WithMessage("OpeningDate is required.")
            .GreaterThan(DateTime.Now);

        RuleFor(x => x.ClosingDate)
            .NotEmpty().WithMessage("ClosingDate is required.")
            .GreaterThan(DateTime.Now)
            .GreaterThan(x => x.OpeningDate);

        RuleFor(x => x.Duration)
            .NotEmpty().WithMessage("Duration is required.")
            .LessThanOrEqualTo(x => x.ClosingDate - x.OpeningDate).WithMessage("Duration has to be less or equal to open time of assignment");
    }
}