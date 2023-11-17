using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;
public class AssignmentTaskService : IAssignmentTaskService
{
    private readonly IRepository<AssignmentTask> _repositoryAssignmentTask;
    private readonly IRepository<Assignment> _repositoryAssignment;
    private readonly IOptionsService _optionsService;
    private readonly IMapper _mapper;

    public AssignmentTaskService(IRepository<AssignmentTask> repositoryAssignmentTask, 
        IRepository<Assignment> repositoryAssignment,
        IOptionsService optionsService,
        IMapper mapper 
        )
    {
        _repositoryAssignmentTask = repositoryAssignmentTask;
        _repositoryAssignment = repositoryAssignment;
        _optionsService = optionsService;
        _mapper = mapper;
    }

    public async Task<AssignmentTaskDTO> AddTask(CreateAssignmentTaskDTO dto)
    {   
        if (dto.Options.Count == 0)
        {
            throw new Exception("No options");
        }

        var entity = _mapper.Map<AssignmentTask>(dto);

        if (await _repositoryAssignment.FirstOrDefaultAsync(assignment => assignment.Id == dto.AssignmentId) == null)
        {
            throw new Exception($"This assingment not in database with this FK: {dto.AssignmentId}");
        }

        await _repositoryAssignmentTask.InsertAsync(entity);
        var options = await _optionsService.AddOptions(entity.Id, dto.Options);

        var result = _mapper.Map<AssignmentTaskDTO>(entity);
        result.Options = options;

        return result;
    }
}
