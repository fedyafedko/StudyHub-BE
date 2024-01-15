using StudyHub.Common.DTO.UserInvitation;

namespace StudyHub.FluentEmail.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync(InvitedUserDTO invitedUserDTO);
}
