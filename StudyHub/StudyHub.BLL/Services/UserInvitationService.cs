using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.Common.Requests;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;
using System.Security.Cryptography;

namespace StudyHub.BLL.Services;

public class UserInvitationService : IUserInvitationService
{
    private readonly IRepository<InvitedUser> _invitedUserRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly EmailSettings _messageSettings;

    public UserInvitationService(
        IRepository<InvitedUser> invitedUserRepository,
        IMapper mapper,
        IEmailService emailService,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptions<EmailSettings> messageSettings)
    {
        _emailService = emailService;
        _userManager = userManager;
        _invitedUserRepository = invitedUserRepository;
        _mapper = mapper;
        _roleManager = roleManager;
        _messageSettings = messageSettings.Value;
    }

    public async Task<bool> InviteManyAsync(Guid userId, InviteUsersRequest dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var roles = await _userManager.GetRolesAsync(user!);

        if (!IsValidRoleToAdd(roles.First(), dto.Role))
        {
            throw new IncorrectParametersException($"Specified user unable to invite users with role: {dto.Role}.");
        }

        var invitedUsers = new List<InvitedUser>();
        var usersMessage = new List<InviteUserMessage>();

        foreach (var email in dto.Emails)
        {
            var allRoles = _roleManager.Roles.ToList().Select(r => r.Name);

            if (!allRoles.Contains(dto.Role))
                throw new NotFoundException($"Role {dto.Role} doesn't exist");

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new IncorrectParametersException($"User with email {email} already exists.");

            var registration = new InvitedUserDTO
            {
                Email = email,
                // ToDo: Move this to Random extensions or something like this
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Role = dto.Role
            };

            var url = string.Format(_messageSettings.AcceptInvitationUrl, registration.Role, registration.Token);

            var userMessage = new InviteUserMessage
            {
                Email = registration.Email,
                Token = registration.Token,
                Role = registration.Role,
                InviteUserUrl = url
            };

            usersMessage.Add(userMessage);

            var invitedUser = _mapper.Map<InvitedUser>(registration);

            invitedUsers.Add(invitedUser);
        }

        await _emailService.SendManyAsync(usersMessage);

        return await _invitedUserRepository.InsertManyAsync(invitedUsers);
    }

    private bool IsValidRoleToAdd(string requestingUserRole, string userToAddRole)
    {
        if (requestingUserRole.ToUpper() == UserRole.Admin.Value)
            return true;

        if (requestingUserRole.ToUpper() == UserRole.Teacher.Value)
            return userToAddRole.ToUpper() == UserRole.Teacher.Value || userToAddRole.ToUpper() == UserRole.Student.Value;

        return false;
    }
}