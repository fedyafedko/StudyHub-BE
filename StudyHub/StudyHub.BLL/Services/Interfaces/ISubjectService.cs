using StudyHub.Common.DTO.Subject;

namespace StudyHub.BLL.Services.Interfaces;

public interface ISubjectService
{
    Task<SubjectDTO> AddSubjectAsync(Guid teacherId, CreateSubjectDTO dto);

    Task<SubjectDTO> GetSubjectAsync(Guid subjectId);

    Task<SubjectDTO> UpdateSubjectAsync(Guid userId, Guid subjectId, UpdateSubjectDTO dto);

    Task<bool> DeleteSubjectAsync(Guid userId, Guid subjectId);

    Task<List<SubjectDTO>> GetSubjectsForUserAsync(Guid userId);
}