using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.BLL.Interfaces;
public interface IAssignmentTaskService
{
    Task<AssignmentTaskDTO> AddTask(CreateAssignmentTaskDTO task);
}
