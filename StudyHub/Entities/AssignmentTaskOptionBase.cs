using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public abstract class AssignmentTaskOptionBase : EntityBase
{
    [ForeignKey(nameof(Task))]
    public Guid AssignmentTaskId { get; set; }

    public string Label { get; set; } = string.Empty;
    public AssignmentTask Task { get; set; } = null!;
}