using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.BLL.Services.Interfaces;

public interface IAssignmentTaskService
{
    Task<AssignmentTaskDTO> AddAssignmentTaskAsync(CreateAssignmentTaskDTO task);

    Task<List<AssignmentTaskDTO>> GetAssignmentTaskAsync(Guid assignmentId);

    Task<AssignmentTaskDTO> UpdateAssignmentTaskAsync(Guid assignmentTaskId, UpdateAssignmentTaskDTO dto);

    Task<bool> DeleteAssignmentTaskAsync(Guid assignmentTaskId);
}