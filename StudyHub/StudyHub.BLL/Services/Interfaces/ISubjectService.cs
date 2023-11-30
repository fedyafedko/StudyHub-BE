using StudyHub.Common.DTO.Subject;

namespace StudyHub.BLL.Services.Interfaces;
public interface ISubjectService
{
    Task<SubjectDTO> AddSubjectAsync(CreateSubjectDTO dto);
    Task<SubjectDTO> GetSubjectAsync(Guid subjectId);
    Task<SubjectDTO> UpdateSubjectAsync(Guid userId, Guid subjectId, UpdateSubjectDTO dto);
    Task<bool> DeleteSubjectAsync(Guid userId, Guid subjectId);
    List<SubjectDTO> GetSubjectsForTeacher(Guid teacherId);
    List<SubjectDTO> GetSubjectsForStudent(Guid studentId);
}
