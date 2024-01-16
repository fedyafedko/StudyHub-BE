using FluentValidation;
using StudyHub.Common.DTO.Assignment;

namespace StudyHub.Validators.AssignmentValidators;

public class UpdateAssignmentVlaidator : AbstractValidator<UpdateAssignmentDTO>
{
    public UpdateAssignmentVlaidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
        RuleFor(x => x.MaxMark)
            .GreaterThan(0);
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.")
            .GreaterThan(DateTime.Now);
        RuleFor(x => x.FinishDate)
            .NotEmpty().WithMessage("FinishDate is required.")
            .GreaterThan(DateTime.Now)
            .GreaterThan(x => x.StartDate);
    }
}