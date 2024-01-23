using AutoMapper;
using Google.Apis.Auth;
using StudyHub.Common.DTO;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.DTO.User;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Entities;

namespace StudyHub.BLL.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<InvitedUserDTO, InvitedUser>();
        CreateMap<RegisterUserDTO, User>()
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<GoogleJsonWebSignature.Payload, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Telegram, opt => opt.MapFrom(src => src.Telegram))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}