using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.Common.DTO.TaskVariant;

namespace StudyHub.Common.DTO.AssignmentTask;

public class AssignmentTaskDTO
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public int MaxMark { get; set; }
    public List<TaskVariantDTO> TaskVariants { get; set; } = new List<TaskVariantDTO>();
}