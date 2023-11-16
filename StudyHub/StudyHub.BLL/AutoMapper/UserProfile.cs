using StudyHub.Entities;
using AutoMapper;
using StudyHub.Common.DTO;

namespace StudyHub.BLL.AutoMapper;
internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterUserDTO>();
    }
}
