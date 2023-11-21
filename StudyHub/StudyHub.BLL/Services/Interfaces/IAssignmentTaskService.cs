using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.BLL.Services.Interfaces;
public interface IAssignmentTaskService
{
    Task<AssignmentTaskDTO> AddTask(CreateAssignmentTaskDTO task);
}
