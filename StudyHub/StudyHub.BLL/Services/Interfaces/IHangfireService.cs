using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;

public interface IHangfireService
{
    bool DeleteUsers(List<InvitedUser> invitedUsers);
}