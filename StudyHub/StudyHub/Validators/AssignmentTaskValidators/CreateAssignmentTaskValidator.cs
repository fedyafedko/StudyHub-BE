using FluentValidation;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Validators.AssignmentTaskOptionValidators;

namespace StudyHub.Validators.AssignmentTaskValidators;

public class CreateAssignmentTaskValidator : AbstractValidator<CreateAssignmentTaskDTO>
{
    public CreateAssignmentTaskValidator()
    {
        RuleFor(x => x.Label).NotEmpty();
        RuleFor(x => x.Mark).GreaterThan(0);
        RuleFor(x => x.Options).SetValidator(new CreateAssignmentTaskOptionValidator());
    }
}
