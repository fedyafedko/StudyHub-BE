using StudyHub.Common.DTO.Subject;

namespace StudyHub.BLL.Interfaces;
public interface ISubjectService
{
    IEnumerable<SubjectDTO> GetSubjects();
    Task<SubjectDTO?> GetSubjectsById(Guid id);
    Task<SubjectDTO> AddSubject(CreateSubjectDTO dto);
    Task<SubjectDTO> UpdateSubject(Guid id, UpdateSubjectDTO dto);
    Task<bool> DeleteSubject(Guid id);
}
