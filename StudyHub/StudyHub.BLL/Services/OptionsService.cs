using AutoMapper;
using StudyHub.BLL.Services.Interfaces;
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

    public async Task<List<AssignmentTaskOptionDTO>> AddOptions(Guid assignmentTaskId, List<CreateAssignmentTaskOptionDTO> taskOptions)
    {
        return await SeparationOptions(assignmentTaskId, taskOptions);
    }
    private async Task<List<AssignmentTaskOptionDTO>> SeparationOptions(
        Guid assignmentTaskId,
        List<CreateAssignmentTaskOptionDTO> taskOptions)
    {
        bool isOpenEnded = taskOptions.All(option => option.IsCorrect != null);

        var options = isOpenEnded
            ? _mapper.Map<List<OpenEndedOption>>(taskOptions).Cast<AssignmentTaskOptionBase>().ToList()
            : _mapper.Map<List<ChoiceOption>>(taskOptions).Cast<AssignmentTaskOptionBase>().ToList();

        options.ForEach(opt => opt.AssignmentTaskId = assignmentTaskId);

        var res = isOpenEnded
            ? await _openEndedOptionRepository.InsertManyAsync(options.OfType<OpenEndedOption>())
            : await _choiceOptionRepository.InsertManyAsync(options.OfType<ChoiceOption>());

        return _mapper.Map<List<AssignmentTaskOptionDTO>>(options);
    }
}
