using StudyHub.Common.DTO.Subject;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface ISubjectService
{
    Task<SubjectDTO> AddSubjectAsync(Guid teacherId, CreateSubjectDTO dto);

    Task<SubjectDTO> GetSubjectAsync(Guid subjectId);

    Task<SubjectDTO> UpdateSubjectAsync(Guid userId, Guid subjectId, UpdateSubjectDTO dto);

    Task<bool> DeleteSubjectAsync(Guid userId, Guid subjectId);

    Task<List<SubjectDTO>> GetSubjectsForUserAsync(Guid userId);

    Task<List<StudentDTO>> AddStudentsToSubjectAsync(Guid subjectId, Guid teacherId, StudentsToSubjectRequest request);
    Task<bool> DeleteStudentsSubjectAsync(Guid subjectId, Guid teacherId, StudentsToSubjectRequest request);
    Task<List<StudentDTO>> GetStudentsForSubjectAsync(Guid subjectId);
}