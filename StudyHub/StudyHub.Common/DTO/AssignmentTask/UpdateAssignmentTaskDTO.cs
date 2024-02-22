using StudyHub.Common.DTO.TaskVariant;

namespace StudyHub.Common.DTO.AssignmentTask;

public class UpdateAssignmentTaskDTO
{
    public int MaxMark { get; set; }
    public List<UpdateTaskVariantDTO> TaskVariants { get; set; } = null!;
}