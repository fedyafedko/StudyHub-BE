using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.Common.Requests;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;
using System.Data.Entity;
using System.Web;

namespace StudyHub.BLL.Services;

public class UserInvitationService : IUserInvitationService
{
    private readonly IRepository<InvitedUser> _invitedUserRepository;
    private readonly IEncryptService _encryptService;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly EmailSettings _messageSettings;
    private readonly IMapper _mapper;

    public UserInvitationService(
        IRepository<InvitedUser> invitedUserRepository,
        IEmailService emailService,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IEncryptService encryptService,
        IOptions<EmailSettings> messageSettings,
        IMapper mapper)
    {
        _invitedUserRepository = invitedUserRepository;
        _emailService = emailService;
        _userManager = userManager;
        _roleManager = roleManager;
        _messageSettings = messageSettings.Value;
        _encryptService = encryptService;
        _mapper = mapper;
    }

    public async Task ClearExpiredInvitations()
    {
        var expired = _invitedUserRepository
            .Where(user => user.CreatedAt.AddDays(7) <= DateTime.Today);

        await _invitedUserRepository.DeleteManyAsync(expired);
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

            string tokenRaw = GenerateTokenExtension.GenerateToken();

            var encodedToken = HttpUtility.UrlEncode(tokenRaw);

            var url = string.Format(_messageSettings.AcceptInvitationUrl, request.Role, encodedToken);

            var userMessage = new InviteUserMessage
            {
                Recipient = email,
                InviteUserUrl = url
            };

            usersMessage.Add(userMessage);

            var registration = new InvitedUserDTO
            {
                Email = email,
                Token = _encryptService.Encrypt(tokenRaw),
                Role = request.Role,
                CreatedAt = DateTime.Today,
            };

            var invitedUser = _mapper.Map<InvitedUser>(registration);

            invitedUsers.Add(invitedUser);
        }

        var IsEmailSend = await _emailService.SendManyAsync(usersMessage);

        if(!IsEmailSend)
            throw new Exception($"Unable to send email.");

        var result = await _invitedUserRepository.InsertManyAsync(invitedUsers);

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