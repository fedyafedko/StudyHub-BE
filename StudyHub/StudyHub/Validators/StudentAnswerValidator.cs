using FluentValidation;
using StudyHub.Common.DTO.User.Student;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.Validators;

public class StudentAnswerValidator : AbstractValidator<StudentAnswerDTO>
{
    private readonly IRepository<TaskOption> _taskOptionRepository;

    public StudentAnswerValidator(IRepository<TaskOption> taskOptionRepository)
    {
        _taskOptionRepository = taskOptionRepository;

        RuleFor(dto => dto)
            .Must(ValidateStudentAnswers)
            .WithMessage("TaskOption does not belong to this variant.");
    }

    private bool ValidateStudentAnswers(StudentAnswerDTO dto)
    {
        foreach (var item in dto.AnswerVariants)
        {
            var options = _taskOptionRepository
                .Where(x => item.TaskOptionIds!.Contains(x.Id))
                .ToList();

            var result = options.All(x => x.TaskVariantId == item.TaskVariantId);

            if(!result)
                return false;
        }

        return true;
    }
}
