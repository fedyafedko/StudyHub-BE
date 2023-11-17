using AutoMapper;
using StudyHub.BLL.Interfaces;
using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;
public class OptionsService : IOptionsService
{
    private readonly IRepository<ChoiceOption> _choiceOptionRepository;
    private readonly IRepository<OpenEndedOption> _openEndedOptionRepository;
    private readonly IMapper _mapper;

    public OptionsService(
        IRepository<ChoiceOption> choiceOptionsRepository,
        IRepository<OpenEndedOption> openEndedOptionsRepository,
        IMapper mapper)
    {
        _choiceOptionRepository = choiceOptionsRepository;
        _openEndedOptionRepository = openEndedOptionsRepository;
        _mapper = mapper;
    }

    public async Task<List<AssignmentTaskOptionDTO>> AddOptions(Guid assignmentTaskId, List<AssignmentTaskOptionDTO> taskOption)
    {
        // ToDo: move this validation to validators?
        if (taskOption.All(option => option.IsCorrect == null))
            return await SeparationOptions(assignmentTaskId,
                taskOption,
                _openEndedOptionRepository);
        else if (taskOption.All(option => option.IsCorrect != null))
            return await SeparationOptions(assignmentTaskId,
                taskOption,
                _choiceOptionRepository);
        else
        {

            var nullOptions = taskOption
                .Where(option => option.IsCorrect == null)
                .ToList();
            var boolOptions = taskOption
                .Where(option => option.IsCorrect.HasValue)
                .ToList();

            var choiceOptionsTask = await SeparationOptions(assignmentTaskId, 
                boolOptions, 
                _choiceOptionRepository);
            var openEndedOptionsTask = await SeparationOptions(assignmentTaskId, 
                nullOptions, 
                _openEndedOptionRepository);

            var result = openEndedOptionsTask
                .Concat(choiceOptionsTask)
                .ToList();

            return result;
        }
    }

    private async Task<List<AssignmentTaskOptionDTO>> SeparationOptions<T>(
        Guid assignmentTaskId,
        List<AssignmentTaskOptionDTO> taskOptions,
        IRepository<T> repository)
            where T : AssignmentTaskOptionBase
    {
        var options = _mapper.Map<List<T>>(taskOptions);
        options.ForEach(opt => opt.AssignmentTaskId = assignmentTaskId);

        await repository.InsertManyAsync(options);

        return _mapper.Map<List<AssignmentTaskOptionDTO>>(options);
    }
}
