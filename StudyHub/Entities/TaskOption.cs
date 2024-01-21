using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class TaskOption : EntityBase
{
    public string? Label { get; set; } = null;
    public bool? IsCorrect { get; set; }
    public TaskVariant TaskVariantOpenEnded { get; set; } = null!;
    public TaskVariant TaskVariantChoiceOption { get; set; } = null!;
    public List<StudentAnswer> StudentAnswers{ get; set; } = null!;

    [ForeignKey(nameof(TaskVariantOpenEnded))]
    public Guid TaskVariantOpenEndedId { get; set; }

    [ForeignKey(nameof(TaskVariantChoiceOption))]
    public Guid TaskVariantChoiceOptionId { get; set; }
}