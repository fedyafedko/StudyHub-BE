using StudyHub.BLL.Services.Interfaces;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class HangfireService : IHangfireService
{
    private readonly IRepository<InvitedUser> _invitedUserRepository;

    public HangfireService(IRepository<InvitedUser> invitedUserRepository)
    {
        _invitedUserRepository = invitedUserRepository;
    }

    public async Task<bool> DeleteUsersAsync(List<InvitedUser> invitedUsers)
    {
        var invitedUserIds = invitedUsers.Select(invitedUser => invitedUser.Id).ToList();

        var remainingUsers = _invitedUserRepository
            .Where(u => invitedUserIds.Contains(u.Id))
            .ToList();

        return await _invitedUserRepository.DeleteManyAsync(remainingUsers);
    }
}