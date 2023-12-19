using AutoMapper;
using FluentEmail.Core;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Security.Cryptography;

namespace StudyHub.BLL.Services;

public class UserInvitedService : IUserInvitedService
{
    private readonly IRepository<InvitedUsers> _invitedUserRepository;
    private readonly IFluentEmail _fluentEmail;
    private readonly IMapper _mapper;
    private static readonly IReadOnlyList<string> Roles = new List<string>
    {
        "Admin",
        "Student",
        "Teacher"
    }.AsReadOnly();

    public UserInvitedService(IRepository<InvitedUsers> invitedUserRepository, IMapper mapper, IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
        _invitedUserRepository = invitedUserRepository;
        _mapper = mapper;
    }

    public async Task InviteAsync(string email, string role)
    {
        if (!Roles.Contains(role))
            throw new NotFoundException($"Role {role} doesn't exist");

        var registration = new InvitedUserDTO
        {
            Email = email,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Role = role
        };
        var sendMessage = await SendInvintationToEmailAsync(registration);

        if (!sendMessage)
            throw new Exception("Something went wrong, invintation was not sent");

        var result = _mapper.Map<InvitedUsers>(registration);

        await _invitedUserRepository.InsertAsync(result);
    }

    public async Task InviteStudentsAsync(InviteStudentsRequest inviteStudentsRequest)
    {
        foreach (var email in inviteStudentsRequest.Email)
        {
            var registration = new InvitedUserDTO
            {
                Email = email,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Role = "Student"
            };

            var sendMessage = await SendInvintationToEmailAsync(registration);

            if (!sendMessage)
                throw new Exception("Something went wrong, invintation was not sent");

            var result = _mapper.Map<InvitedUsers>(registration);

            await _invitedUserRepository.InsertAsync(result);
        }
    }

    public async Task<bool> SendInvintationToEmailAsync( InvitedUserDTO invitedUserDTO)
    {
        var messageBuilder = new EmailMessageBuilder();

        string emailBody = messageBuilder.BuildInvitationEmail(invitedUserDTO);

        var sendEmail = await _fluentEmail
             .To(invitedUserDTO.Email)
             .Subject("Invitation")
             .Body(emailBody)
             .SendAsync();

        _fluentEmail.Data.ToAddresses.Clear();

        return sendEmail.Successful;
    }
}
