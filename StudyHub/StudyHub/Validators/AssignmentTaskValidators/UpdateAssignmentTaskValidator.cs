using FluentValidation;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Validators.AssignmentTaskOptionValidators;

namespace StudyHub.Validators.AssignmentTaskValidators;

public class UpdateAssignmentTaskValidator : AbstractValidator<UpdateAssignmentTaskDTO>
{
    public UpdateAssignmentTaskValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty();
            
        RuleFor(x => x.Mark).GreaterThan(0);
        RuleFor(x => x.Options)
            .SetValidator(new UpdateAssignmentTaskOptionValidator());
    }
}
