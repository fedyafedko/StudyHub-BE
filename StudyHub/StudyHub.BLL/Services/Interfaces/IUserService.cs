using StudyHub.Common.DTO.User;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface IUserService
{
    Task<PageList<StudentDTO>> GetStudentsAsync(SearchRequest request);
    Task<UserDTO> UpdateUserAsync(Guid userId, UpdateUserDTO dto);
    Task<UserDTO> GetUserAsync(Guid userId);
}
