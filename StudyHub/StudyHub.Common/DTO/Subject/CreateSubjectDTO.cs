namespace StudyHub.Common.DTO.Subject;

public class CreateSubjectDTO
{
    public Guid TeacherId { get; set; }
    public string Title { get; set; } = string.Empty;
}