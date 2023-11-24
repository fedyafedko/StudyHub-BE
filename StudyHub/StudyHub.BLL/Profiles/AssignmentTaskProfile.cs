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
            .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
            .ForMember(dest => dest.Label, source => source.MapFrom(x => x.Label))
            .ForMember(dest => dest.Mark, source => source.MapFrom(x => x.Mark))
            .ForMember(dest => dest.Options, source => source.Ignore());
        CreateMap<UpdateAssignmentTaskDTO, AssignmentTask>()
            .ForMember(dest => dest.Label, source => source.MapFrom(x => x.Label))
            .ForMember(dest => dest.Mark, source => source.MapFrom(x => x.Mark))
            .ForMember(dest => dest.Options, source => source.Ignore());
    }
}
