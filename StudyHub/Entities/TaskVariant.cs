using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class TaskVariant : EntityBase
{
    public string Label { get; set; } = string.Empty;
    public AssignmentTask AssignmentTask { get; set; } = null!;
    public List<TaskOption> ChoiceOption { get; set; } = null!;
    public List<TaskOption> OpenEndedOption { get; set; } = null!;
    public List <StudentAnswer> StudentAnswers { get; set; } = null!;

    [ForeignKey(nameof(AssignmentTask))]
    public Guid AssignmentTaskId { get; set; }  
}
