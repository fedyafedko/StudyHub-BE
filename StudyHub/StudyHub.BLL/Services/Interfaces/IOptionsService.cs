using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.BLL.Services.Interfaces;
public interface IOptionsService
{
    Task<List<AssignmentTaskOptionDTO>> AddOptions(Guid taskId, List<CreateAssignmentTaskOptionDTO> taskOption);
}
