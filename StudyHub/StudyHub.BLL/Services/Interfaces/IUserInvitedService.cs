using StudyHub.Common.DTO;

namespace StudyHub.BLL.Services.Interfaces;
public interface IUserInvitedService
{
    Task InviteStudentsAsync(InviteStudentsRequest inviteStudentsRequest);
    Task InviteAsync(string email, string role);
    Task<bool> SendInvintationToEmailAsync(InvitedUserDTO invitedUserDTO);
}
