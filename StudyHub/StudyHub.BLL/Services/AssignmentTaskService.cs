using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;
public class AssignmentTaskService : IAssignmentTaskService
{
    private readonly IRepository<AssignmentTask> _assignmentTaskRepository;
    private readonly IRepository<Assignment> _assignmentRepository;
    private readonly IOptionsService _optionsService;
    private readonly IMapper _mapper;

    public AssignmentTaskService(
        IRepository<AssignmentTask> repositoryAssignmentTask, 
        IRepository<Assignment> repositoryAssignment,
        IOptionsService optionsService,
        IMapper mapper)
    {
        _assignmentTaskRepository = repositoryAssignmentTask;
        _assignmentRepository = repositoryAssignment;
        _optionsService = optionsService;
        _mapper = mapper;
    }

    public async Task<AssignmentTaskDTO> AddTask(CreateAssignmentTaskDTO dto)
    {   
        var entity = _mapper.Map<AssignmentTask>(dto);

        var assignment = await _assignmentRepository.FirstOrDefaultAsync(assignment => assignment.Id == dto.AssignmentId);

        if (assignment == null)
        {
            throw new AssignmentNotFoundException($"Assignment not found in the database with this ID: {dto.AssignmentId}");
        }

        await _assignmentTaskRepository.InsertAsync(entity);
        var options = await _optionsService.AddOptions(entity.Id, dto.Options);

        var result = _mapper.Map<AssignmentTaskDTO>(entity);
        result.Options = options;

        return result;
    }
}
