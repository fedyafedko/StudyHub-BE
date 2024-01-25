using StudyHub.Common.DTO.User;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface IUserService
{
    Task<PageList<StudentDTO>> GetStudents(SearchRequest request);
    Task<UserDTO> UpdateUserAsync(Guid userId, UpdateUserDTO dto);
}
