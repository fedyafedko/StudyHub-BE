﻿using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.Common.DTO.TaskVariant;

namespace StudyHub.BLL.Services.Interfaces.Assignment;

public interface IOptionsService
{
    Task<List<TaskOptionDTO>> AddTaskOptionsAsync(Guid taskVariantId, List<CreateTaskOptionDTO> dto);

    Task<TaskOptionDTO> UpdateTaskOptionsAsync(Guid optionId, UpdateTaskOptionDTO dto);

    Task<bool> DeleteTaskOptionsAsync(Guid optionId);
}