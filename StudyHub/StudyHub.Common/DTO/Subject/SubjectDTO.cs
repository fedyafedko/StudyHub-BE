namespace StudyHub.Common.DTO.Subject;

public class SubjectDTO
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public string Title { get; set; } = string.Empty;
}