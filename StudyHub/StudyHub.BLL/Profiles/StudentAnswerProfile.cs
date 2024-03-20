using AutoMapper;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Entities;

namespace StudyHub.BLL.Profiles;

public class StudentAnswerProfile : Profile
{
    public StudentAnswerProfile()
    {
        CreateMap<AnswerVariantDTO, StudentAnswer>()
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
