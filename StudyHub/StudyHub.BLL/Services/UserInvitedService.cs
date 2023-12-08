using AutoMapper;
using FluentEmail.Core;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Security.Cryptography;
using System.Text;

namespace StudyHub.BLL.Services;

public class UserInvitedService : IUserInvitedService
{
    private readonly IRepository<InvitedUsers> _invitedUserRepository;
    private readonly IFluentEmail _fluentEmail;
    private readonly IMapper _mapper;
    public UserInvitedService(IRepository<InvitedUsers> invitedUserRepository, IMapper mapper, IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
        _invitedUserRepository = invitedUserRepository;
        _mapper = mapper;
    }

    public async Task CreateRegistrationUrl(string email, string role)
    {   
        var registration = new InvitedUserDTO
        {
            Email = email,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        };

        registration.Role = role switch
        {
            "Teacher" => "Student",
            "Admin" => "Teacher",
            _ => throw new NotFoundException("Some role can't be handle")
        };

        string url = $"https://localhost:1234/accept-invitation?role={registration.Role}?token={registration.Token}";

        StringBuilder template = new();
        template.AppendLine($"Dear {email}");
        template.AppendLine($"You were invited to the course by {role}, your invintation url: {url}");
        template.AppendLine("Sincerely, our dev team!");

        var sendEmail = await _fluentEmail
             .To(email)
             .Subject("Invitation")
             .Body(template.ToString())
             .SendAsync();

        var result = _mapper.Map<InvitedUsers>(registration);
        
        await _invitedUserRepository.InsertAsync(result);
    }
}
