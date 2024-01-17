using AutoMapper;
using Hangfire;
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
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly EmailSettings _messageSettings;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IMapper _mapper;

    public UserInvitationService(
        IRepository<InvitedUser> invitedUserRepository,
        IHangfireService hangfireService,
        IEmailService emailService,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptions<EmailSettings> messageSettings,
        IBackgroundJobClient backgroundJobClient,
        IMapper mapper)
    {
        _invitedUserRepository = invitedUserRepository;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _userManager = userManager;
        _roleManager = roleManager;
        _messageSettings = messageSettings.Value;
        _backgroundJobClient = backgroundJobClient;
        _mapper = mapper;
    }

    public async Task<bool> InviteManyAsync(Guid userId, InviteUsersRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var roles = await _userManager.GetRolesAsync(user!);

        if (!IsValidRoleToAdd(roles.First(), request.Role))
        {
            throw new IncorrectParametersException($"Specified user unable to invite users with role: {request.Role}.");
        }

        var invitedUsers = new List<InvitedUser>();
        var usersMessage = new List<InviteUserMessage>();

        foreach (var email in request.Emails)
        {
            var allRoles = _roleManager.Roles.ToList().Select(r => r.Name);

            if (!allRoles.Contains(request.Role))
                throw new NotFoundException($"Role {request.Role} doesn't exist");

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new IncorrectParametersException($"User with email {email} already exists.");

            string tokenRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // ToDo: place html friendly token as param here
            var url = string.Format(_messageSettings.AcceptInvitationUrl, request.Role, tokenRaw);

            var userMessage = new InviteUserMessage
            {
                Recipient = email,
                InviteUserUrl = url
            };

            usersMessage.Add(userMessage);

            var registration = new InvitedUserDTO
            {
                Email = email,
                // ToDo: Move this to Random extensions or something like this + encode via HtmlUtility
                Token = tokenRaw,
                Role = request.Role
            };
            var invitedUser = _mapper.Map<InvitedUser>(registration);

            invitedUsers.Add(invitedUser);
        }

        var IsEmailSend = await _emailService.SendManyAsync(usersMessage);

        if(!IsEmailSend)
            throw new Exception($"Unable to send email.");
        
        var result = await _invitedUserRepository.InsertManyAsync(invitedUsers);

        _backgroundJobClient.Schedule(() => _hangfireService.DeleteUsersAsync(invitedUsers), TimeSpan.FromDays(7));

        return result;
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