using FluentValidation;
using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.Validators;

public class CreateAssignmentTaskValidator : AbstractValidator<CreateAssignmentTaskDTO>
{
    public CreateAssignmentTaskValidator()
    {
        RuleFor(x => x.Label).NotEmpty();
        RuleFor(x => x.Mark).GreaterThan(0);
        RuleFor(x => x.Options).SetValidator(new CreateAssignmentTaskOptionValidator());
    }
}

public class UpdateAssignmentTaskValidator : AbstractValidator<UpdateAssignmentTaskDTO>
{
    public UpdateAssignmentTaskValidator()
    {
        RuleFor(x => x.Label).NotEmpty();
        RuleFor(x => x.Mark).GreaterThan(0);
        RuleFor(x => x.Options).SetValidator(new UpdateAssignmentTaskOptionValidator());
    }
}
