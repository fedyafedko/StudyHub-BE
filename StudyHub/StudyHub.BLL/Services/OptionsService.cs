using AutoMapper;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class OptionsService : IOptionsService
{
    public Task<List<AssignmentTaskOptionDTO>> AddAssignmentTaskOptionsAsync(Guid taskId, List<CreateAssignmentTaskOptionDTO> dto)
    {
        throw new NotImplementedException();
    }

    public Task<List<AssignmentTaskOptionDTO>> UpdateAssignmentTaskOptionsAsync(Guid taskId, List<UpdateAssignmentTaskOptionDTO> dto)
    {
        throw new NotImplementedException();
    }
}