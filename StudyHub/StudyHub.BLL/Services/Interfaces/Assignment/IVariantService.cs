using StudyHub.Common.DTO.TaskVariant;

namespace StudyHub.BLL.Services.Interfaces.Assignment;

public interface IVariantService
{
    Task<TaskVariantDTO> CreateTaskVariantAsync(Guid assignmentTaskId, CreateTaskVariantDTO Variants);

    Task<List<TaskVariantDTO>> GetTaskVariantAsync(Guid assignmentTaskId);

    Task<bool> DeleteTaskVariantAsync(Guid taskVariantId);
}
