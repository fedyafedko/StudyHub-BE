using AutoMapper;
using StudyHub.Entities;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Common.DTO.AssignmentTaskOption;

namespace StudyHub.BLL.Profiles;

public class AssignmentTaskProfile : Profile
{
    public AssignmentTaskProfile()
    {
        CreateMap<AssignmentTask, AssignmentTaskDTO>();
        CreateMap<CreateAssignmentTaskDTO, AssignmentTask>()
            .ForMember(dest => dest.Options, source => source.Ignore());
        CreateMap<UpdateAssignmentTaskDTO, AssignmentTask>()
            .ForMember(dest => dest.Options, source => source.Ignore());
    }
}
