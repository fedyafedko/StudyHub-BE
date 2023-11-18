using StudyHub.Entities;
using AutoMapper;
using StudyHub.Common.DTO;

namespace StudyHub.BLL.AutoMapper;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDTO, User>();
        CreateMap<LoginUserDTO, User>();
    }
}
