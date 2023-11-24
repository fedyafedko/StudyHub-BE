using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.BLL.Services.Interfaces;
public interface IAssignmentTaskService
{
    Task<AssignmentTaskDTO> AddAssignmentTask(CreateAssignmentTaskDTO task);
    Task<List<AssignmentTaskDTO>> GetAssignmentTask(Guid assignmentId);
    Task<AssignmentTaskDTO> UpdateAssignmentTask(Guid assignmentTaskId, UpdateAssignmentTaskDTO dto);
    Task<bool> DeleteAssignmentTask(Guid assignmentTaskId);

}
