using FluentValidation;
using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.Validators.AssignmentTaskOptionValidators;

public class UpdateAssignmentTaskOptionValidator : AbstractValidator<List<UpdateAssignmentTaskOptionDTO>>
{
    public UpdateAssignmentTaskOptionValidator()
    {
        RuleFor(options => options)
            .Must(HaveConsistentCorrectness)
            .WithMessage("Options must all have IsCorrect set or unset, but not a mix.");

        RuleFor(options => options)
            .Must(options => options.Count > 0)
            .WithMessage("Options must not be empty");
    }

    private bool HaveConsistentCorrectness(List<UpdateAssignmentTaskOptionDTO> options)
    {
        bool IsNotNull = options.All(option => option.IsCorrect != null);
        bool IsNull = options.All(option => option.IsCorrect == null);

        return IsNotNull || IsNull;
    }
}