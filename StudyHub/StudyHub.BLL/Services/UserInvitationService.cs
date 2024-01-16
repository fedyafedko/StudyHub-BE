using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common;
using StudyHub.Common.DTO;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using StudyHub.FluentEmail.Interfaces;
using System.Security.Cryptography;

namespace StudyHub.BLL.Services;

public class UserInvitationService : IUserInvitationService
{
    private readonly IRepository<InvitedUser> _invitedUserRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserInvitationService(
        IRepository<InvitedUser> invitedUserRepository,
        IMapper mapper,
        IEmailService emailService,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _emailService = emailService;
        _userManager = userManager;
        _invitedUserRepository = invitedUserRepository;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<bool> InviteManyAsync(Guid userId, InviteUsersDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var roles = await _userManager.GetRolesAsync(user!);

        if (!IsValidRoleToAdd(roles.First(), dto.Role))
        {
            throw new IncorrectParametersException($"Specified user unable to invite users with role: {dto.Role}.");
        }

        var invitedUsers = new List<InvitedUser>();

        foreach (var email in dto.Emails)
        {
            var allRoles = _roleManager.Roles.ToList().Select(r => r.Name);

            if (!allRoles.Contains(dto.Role))
                throw new NotFoundException($"Role {dto.Role} doesn't exist");

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new IncorrectParametersException($"User with email {email} already exists.");

            var userToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var registration = new InvitedUserDTO
            {
                Email = email,
                Token = userToken,
                Role = dto.Role
            };

            var isMessageSent = await _emailService.SendAsync(registration);

            if (!isMessageSent)
                throw new Exception($"Unable to send email. Email: {email}");

            registration.Token = userToken.Encrypt();

            var invitedUser = _mapper.Map<InvitedUser>(registration);

            invitedUsers.Add(invitedUser);
        }

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
