using FluentValidation;
using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.Validators;

public class AssignmentTaskValidator : AbstractValidator<CreateAssignmentTaskDTO>
{
    public AssignmentTaskValidator()
    {
        RuleFor(x => x.Label).NotEmpty();
        RuleFor(x => x.Mark).GreaterThan(0);
        RuleFor(x => x.Options).SetValidator(new AssignmentTaskOptionValidator());
    }
}
