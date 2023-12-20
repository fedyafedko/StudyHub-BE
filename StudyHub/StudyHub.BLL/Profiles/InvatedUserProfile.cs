using AutoMapper;
using StudyHub.Common.DTO;
using StudyHub.Entities;

namespace StudyHub.BLL.Profiles;

public class InvatedUserProfile : Profile
{
    public InvatedUserProfile() 
    {
        CreateMap<InvitedUserDTO, InvitedUser>();
    }
}
