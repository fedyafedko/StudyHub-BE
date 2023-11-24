using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.BLL.Services.Interfaces;
public interface IOptionsService
{
    Task<List<AssignmentTaskOptionDTO>> AddOptions(Guid taskId, List<CreateAssignmentTaskOptionDTO> taskOption);
    Task<List<AssignmentTaskOptionDTO>> UpdateApartmentTaskOption(Guid taskId, List<UpdateAssignmentTaskOptionDTO> dto);

}
