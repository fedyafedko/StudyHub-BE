using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.Common.DTO.AssignmentTask;

public class CreateAssignmentTaskDTO
{
    public Guid AssignmentId { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Mark { get; set; }
    public List<CreateAssignmentTaskOptionDTO> Options { get; set; } = new List<CreateAssignmentTaskOptionDTO>();
}