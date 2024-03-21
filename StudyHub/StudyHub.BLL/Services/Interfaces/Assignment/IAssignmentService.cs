using StudyHub.Common.DTO.Assignment;

namespace StudyHub.BLL.Services.Interfaces.Assignment;

public interface IAssignmentService
{
    Task<AssignmentDTO> CreateAssignmentAsync(CreateAssignmentDTO dto);

    Task<AssignmentDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentDTO dto);

    Task<bool> DeleteAssignmentAsync(Guid assignmentId);

    Task<AssignmentDTO> GetAssignmentByIdAsync(Guid assignmentId);

    Task<List<AssignmentDTO>> GetAssignmentsBySubjectIdAsync(Guid subjectId);
    Task<AssignmentDTO> GetNextAssignmentAsync(Guid userId);
}