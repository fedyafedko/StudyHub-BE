using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface IStudentSubjectService
{
    Task<List<StudentDTO>> AddStudentsToSubjectAsync(Guid subjectId, Guid teacherId, StudentsToSubjectRequest request);
    Task<bool> DeleteStudentsSubjectAsync(Guid subjectId, Guid teacherId, StudentsToSubjectRequest request);
    Task<List<StudentDTO>> GetStudentsForSubjectAsync(Guid subjectId);
}
