using AutoMapper;
using StudyHub.Common.DTO;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Entities;

namespace StudyHub.BLL.Profiles;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<InvitedUserDTO, InvitedUser>();
        CreateMap<RegisterUserDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(_ => true));
    }
}
