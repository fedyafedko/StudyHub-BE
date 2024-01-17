using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;

public interface IHangfireService
{
    Task<bool> DeleteUsersAsync(List<InvitedUser> invitedUsers);
}