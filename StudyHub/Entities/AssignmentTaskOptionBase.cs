using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyHub.Entities;

public class AssignmentTaskOptionBase
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Task))]
    public Guid AssignmentTaskId { get; set; }
    public string Label { get; set; } = string.Empty;
    public AssignmentTask Task { get; set; } = null!;
}