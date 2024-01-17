using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.BLL.Services.Interfaces;

public interface IOptionsService
{
    Task<List<AssignmentTaskOptionDTO>> AddAssignmentTaskOptionsAsync(Guid taskId, List<CreateAssignmentTaskOptionDTO> dto);

    Task<List<AssignmentTaskOptionDTO>> UpdateAssignmentTaskOptionsAsync(Guid taskId, List<UpdateAssignmentTaskOptionDTO> dto);
}