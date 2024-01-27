namespace StudyHub.Common.DTO.AssignmentTaskOption;

public class CreateTaskOptionDTO
{
    public string Label { get; set; } = string.Empty;
    public bool? IsCorrect { get; set; }
}