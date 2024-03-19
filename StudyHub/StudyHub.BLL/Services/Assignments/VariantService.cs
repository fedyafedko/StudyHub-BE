using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.TaskVariant;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Assignments;

public class VariantService : IVariantService
{
    private readonly IMapper _mapper;   
    private readonly IRepository<TaskVariant> _taskVariantRepository;
    private readonly IRepository<AssignmentTask> _assignmentTaskRepository;

    public VariantService(IMapper mapper, 
        IRepository<TaskVariant> taskVariantRepository, 
        IRepository<AssignmentTask> assignmentTaskRepository)
    {
        _mapper = mapper;
        _taskVariantRepository = taskVariantRepository;
        _assignmentTaskRepository = assignmentTaskRepository;
    }

    public async Task<TaskVariantDTO> CreateTaskVariantAsync(Guid assignmentTaskId, CreateTaskVariantDTO taskVariant)
    {
        var assignmentTask = await _assignmentTaskRepository.FirstOrDefaultAsync(assignmentTask => assignmentTask.Id == assignmentTaskId)
            ?? throw new NotFoundException($"Task with this Id is not found: {assignmentTaskId}");

        var variant = _mapper.Map<TaskVariant>(taskVariant);

        variant.AssignmentTaskId = assignmentTaskId;

        await _taskVariantRepository.InsertAsync(variant);

        var result = _mapper.Map<TaskVariantDTO>(variant);

        return result;
    }

    public async Task<bool> DeleteTaskVariantAsync(Guid taskVariantId)
    {
        var entity = await _taskVariantRepository.FirstOrDefaultAsync(taskVariant => taskVariant.Id == taskVariantId)
            ?? throw new NotFoundException($"Task variant with this Id is not found: {taskVariantId}");

        return await _taskVariantRepository.DeleteAsync(entity);
    }

    public async Task<List<TaskVariantDTO>> GetTaskVariantAsync(Guid assignmentTaskId)
    {
        var result = await _taskVariantRepository
            .Include(taskVariant => taskVariant.TaskOption)
            .Where(taskVariant => taskVariant.AssignmentTaskId == assignmentTaskId)
            .ToListAsync();
        
        if(result.Count == 0)
            throw new NotFoundException($"Unable to find entity with such key {assignmentTaskId}");

        return _mapper.Map<List<TaskVariantDTO>>(result);
    }

    public async Task<TaskVariantDTO> UpdateTaskVariantAsync(Guid taskVariantId, UpdateTaskVariantDTO taskVariant)
    {
        var entity = await _taskVariantRepository
            .Include(taskVariant => taskVariant.TaskOption)
            .FirstOrDefaultAsync(taskVariant => taskVariant.Id == taskVariantId)
            ?? throw new NotFoundException($"Task variant with this Id is not found: {taskVariantId}");

        _mapper.Map(taskVariant, entity);

        await _taskVariantRepository.UpdateAsync(entity);

        return _mapper.Map<TaskVariantDTO>(entity);
    }
}
