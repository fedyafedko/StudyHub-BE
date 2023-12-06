using AutoMapper;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Security.Cryptography;

namespace StudyHub.BLL.Services;
public class UserInvitedService : IUserInvitedService
{
    private readonly IRepository<InvitedUsers> _repoInvitedUsers;
    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

    private readonly IMapper _mapper;
    public UserInvitedService(IRepository<InvitedUsers> repoInvitedUsers, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _repoInvitedUsers = repoInvitedUsers;
        _mapper = mapper;
    }

    public async Task<bool> CreateRegistrationUrl(string email, string role)
    {   
        var registration = new InvatedUserDTO
        {
            Email = email,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        };
        if (role == "Teacher")
             registration.Role = "Student";

        if (role == "Admin")
            registration.Role = "Teacher";

        var result = _mapper.Map<InvitedUsers>(registration);

        if(await _repoInvitedUsers.InsertAsync(result))
            return true;

        return false;
    }
}
