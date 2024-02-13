using StudyHub.Common.DTO.User.Student;

namespace StudyHub.BLL.Services.Interfaces;

public interface IStudentAnswerService
{
    Task<bool> UpsertStudentAnswerAsync(Guid studentId, StudentAnswerDTO dto);
}
