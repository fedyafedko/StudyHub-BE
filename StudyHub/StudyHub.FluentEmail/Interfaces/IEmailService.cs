using StudyHub.Common.DTO;

namespace StudyHub.FluentEmail.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync(InvitedUserDTO invitedUserDTO);
}
