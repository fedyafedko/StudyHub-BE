using StudyHub.Common.DTO.Subject;

namespace StudyHub.BLL.Services.Interfaces;
public interface ISubjectService
{
    Task<SubjectDTO> AddSubjectAsync(CreateSubjectDTO dto);
    Task<SubjectDTO> GetSubjectAsync(Guid subjectId);
    Task<SubjectDTO> UpdateSubjectAsync(Guid subjectId, UpdateSubjectDTO dto);
    Task<bool> DeleteSubjectAsync(Guid subjectId);
}
