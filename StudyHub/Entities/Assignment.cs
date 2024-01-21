using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class Assignment : EntityBase
{
    [ForeignKey(nameof(Subject))]
    public Guid SubjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int MaxMark { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public TimeSpan Duration { get; set; }
    public Subject Subject { get; set; } = null!;
    public List<AssignmentTask> Tasks { get; set; } = null!;
}