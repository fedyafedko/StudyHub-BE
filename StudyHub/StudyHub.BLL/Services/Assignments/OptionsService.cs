using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Assignments;

public class OptionsService : IOptionsService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TaskOption> _taskOptionRepository;

    public OptionsService(IMapper mapper, IRepository<TaskOption> taskOptionsRepository)
    {
        _mapper = mapper;
        _taskOptionRepository = taskOptionsRepository;
    }

    public async Task<List<TaskOptionDTO>> AddTaskOptionsAsync(Guid taskVariantId, List<CreateTaskOptionDTO> taskOptions)
    {
        var options = _mapper.Map<List<TaskOption>>(taskOptions).ToList();

        options.ForEach(opt => opt.TaskVariantId = taskVariantId);

        await _taskOptionRepository.InsertManyAsync(options);

        return _mapper.Map<List<TaskOptionDTO>>(options);
    }

    public async Task<bool> DeleteTaskOptionsAsync(Guid optionId)
    {
        var entity = await _taskOptionRepository.FirstOrDefaultAsync(x => x.Id == optionId)
           ?? throw new NotFoundException($"Unable to find entity with this key: {optionId}");

        return await _taskOptionRepository.DeleteAsync(entity);
    }
}