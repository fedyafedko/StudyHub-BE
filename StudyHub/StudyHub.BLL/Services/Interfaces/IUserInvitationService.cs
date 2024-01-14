using StudyHub.Common.DTO;
using StudyHub.Common.DTO.UserInvitation;

namespace StudyHub.BLL.Services.Interfaces;

public interface IUserInvitationService
{
    Task<bool> InviteManyAsync(Guid userId, InviteUsersDTO dto);
}
