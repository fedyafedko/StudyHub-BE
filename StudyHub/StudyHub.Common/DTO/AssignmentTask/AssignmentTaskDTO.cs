using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.Common.DTO.AssignmentTask;

public class AssignmentTaskDTO
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Mark { get; set; }
    public List<AssignmentTaskOptionDTO> Options { get; set; } = new List<AssignmentTaskOptionDTO>();
}
