using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.User.Student;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace StudyHub.BLL.Services;

public class StudentAnswerService : IStudentAnswerService
{
    private readonly IRepository<StudentAnswer> _studentAnswerRepository;
    private readonly IRepository<Assignment> _assignmentRepository;
    private readonly IRepository<TaskOption> _taskOptionRepository;
    private readonly IRepository<StartingTimeRecord> _startingTimeRepository;
    private readonly IMapper _mapper;

    public StudentAnswerService(
        IRepository<StudentAnswer> studentAnswerRepository,
        IRepository<Assignment> assignmentRepository,
        IRepository<TaskOption> taskOptionRepository,
        IRepository<StartingTimeRecord> startingTimeRepository,
        IMapper mapper)
    {
        _studentAnswerRepository = studentAnswerRepository;
        _assignmentRepository = assignmentRepository;
        _taskOptionRepository = taskOptionRepository;
        _startingTimeRepository = startingTimeRepository;
        _mapper = mapper;
    }

    public async Task<bool> UpsertStudentAnswerAsync(Guid studentId, StudentAnswerDTO dto)
    {
        var validate = await ValidateStudentAnswersAsync(dto);

        if(!validate)
            throw new ValidationException("TaskOption does not belong to this variant.");

        var startTime = await _startingTimeRepository.FirstOrDefaultAsync(x => x.StudentId == studentId)
            ?? throw new NotFoundException("Starting time not found");

        var duration = await _assignmentRepository
            .Where(x => x.Id == dto.AssignmentId)
            .Select(x => x.Duration)
            .FirstOrDefaultAsync();

        var isTimeOver = DateTime.Now - startTime.StartTime < duration;

        if (!isTimeOver)
            throw new TimeOverException("Time is over");

        var studentAnswers = await _studentAnswerRepository
            .Where(x => x.StudentId == studentId && x.TaskVariant.AssignmentTask.AssignmentId == dto.AssignmentId)
            .Include(x => x.TaskOptions)
            .ToListAsync();

        var inserts = dto.AnswerVariants
            .Where(item => !studentAnswers.Any(x => x.TaskVariantId == item.TaskVariantId))
            .ToList();

        await InsertAnswersAsync(studentId, inserts);

        studentAnswers = await _studentAnswerRepository
            .Where(x => x.StudentId == studentId && x.TaskVariant.AssignmentTask.AssignmentId == dto.AssignmentId)
            .Include(x => x.TaskOptions)
            .ToListAsync();

        foreach (var studentAnswer in studentAnswers)
        {
            var answerVariant = dto.AnswerVariants.FirstOrDefault(d => d.TaskVariantId == studentAnswer.TaskVariantId);
            if (answerVariant != null)
            {
                studentAnswer.Answer = answerVariant.Answer;
            }
        }

        studentAnswers.ForEach(x => x.TaskOptions = new List<TaskOption>());

        var result = ProcessStudentAnswers(studentAnswers, dto.AnswerVariants);

        await _studentAnswerRepository.UpdateManyAsync(result);

        return true;
    }

    private async Task<bool> InsertAnswersAsync(Guid studentId, List<AnswerVariantDTO> dto)
    {
        var answers = _mapper.Map<List<StudentAnswer>>(dto);

        answers.ForEach(x =>
        {
            x.StudentId = studentId;
            x.TaskOptions ??= new List<TaskOption>();
        });

        var result = ProcessStudentAnswers(answers, dto);

        await _studentAnswerRepository.InsertManyAsync(result);

        return true;
    }

    private List<StudentAnswer> ProcessStudentAnswers(
        List<StudentAnswer> answers,
        List<AnswerVariantDTO> dto)
    {
        var openEnded = answers.Where(x => x.Answer != null).ToList();
        var choiceOptions = answers.Where(x => x.Answer == null).ToList();

        var taskOptions = new List<TaskOption>();

        var choiceOptionsDTO = dto.Where(x => x.Answer == null).ToList();

        var options = choiceOptionsDTO
                    .Select(option => _taskOptionRepository.Where(x => option.TaskOptionIds!.Contains(x.Id)))
                    .SelectMany(_ => _)
                    ?? throw new NotFoundException("Task options not found");

        taskOptions.AddRange(options);

        choiceOptions.ForEach(x => x.TaskOptions.AddRange(taskOptions));

        var result = choiceOptions.Concat(openEnded).ToList();

        return result;
    }

    private async Task<bool> ValidateStudentAnswersAsync(StudentAnswerDTO dto)
    {
        foreach (var item in dto.AnswerVariants)
        {
            if (item.TaskOptionIds == null)
                continue;

            var query = _taskOptionRepository
                .Where(x => item.TaskOptionIds.Contains(x.Id));

            var options = await query.ToListAsync();

            var result = options.All(x => x.TaskVariantId == item.TaskVariantId);

            if (!result)
                return false;
        }

        return true;
    }
}
