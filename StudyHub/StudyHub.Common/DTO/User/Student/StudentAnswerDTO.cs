namespace StudyHub.Common.DTO.User.Student;

public class StudentAnswerDTO
{
    public Guid AssignmentId { get; set; }
    public List<AnswerVariantDTO> AnswerVariants { get; set; } = null!;
}

public class AnswerVariantDTO
{
    public Guid TaskVariantId { get; set; }
    public string? Answer { get; set; }
    public List<Guid>? TaskOptionIds { get; set; }
}
