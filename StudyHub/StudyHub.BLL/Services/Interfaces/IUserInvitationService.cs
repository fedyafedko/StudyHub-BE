using StudyHub.Common.DTO;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface IUserInvitationService
{
    Task ClearExpiredInvitationsAsync();
    Task<bool> InviteManyAsync(Guid userId, InviteUsersRequest dto);
}
