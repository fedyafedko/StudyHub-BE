using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.Assignment;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Assignments;

public class AssignmentService : IAssignmentService
{
    private readonly IRepository<Assignment> _assignmentRepository;
    private readonly IRepository<Subject> _subjectRepository;
    private readonly IRepository<StudentAnswer> _studentAnswerRepository;
    private readonly IRepository<StartingTimeRecord> _startingTimeRecordRepository;
    private readonly IMapper _mapper;

    public AssignmentService(
        IRepository<Assignment> assignmentRepository,
        IRepository<Subject> subjectRepository,
        IMapper mapper,
        IRepository<StudentAnswer> studentAnswerRepository,
        IRepository<StartingTimeRecord> startingTimeRecordRepository)
    {
        _assignmentRepository = assignmentRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _studentAnswerRepository = studentAnswerRepository;
        _startingTimeRecordRepository = startingTimeRecordRepository;
    }

    public async Task<AssignmentDTO> CreateAssignmentAsync(CreateAssignmentDTO assignment)
    {
        var entity = _mapper.Map<Assignment>(assignment);

        var subject = await _subjectRepository.FirstOrDefaultAsync(x => x.Id == assignment.SubjectId)
            ?? throw new NotFoundException($"Subject not found in the database with this Id: {assignment.SubjectId}");

        await _assignmentRepository.InsertAsync(entity);

        var result = _mapper.Map<AssignmentDTO>(entity);

        return result;
    }

    public async Task<bool> DeleteAssignmentAsync(Guid assignmentId)
    {
        var entity = await _assignmentRepository
            .FirstOrDefaultAsync(x => x.Id == assignmentId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {assignmentId}");

        return await _assignmentRepository.DeleteAsync(entity);
    }

    public async Task<AssignmentDTO> GetAssignmentByIdAsync(Guid assignmentId)
    {
        var entity = await _assignmentRepository
            .FirstOrDefaultAsync(x => x.Id == assignmentId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {assignmentId}");

        return _mapper.Map<AssignmentDTO>(entity);
    }

    public async Task<List<AssignmentDTO>> GetAssignmentsBySubjectIdAsync(Guid subjectId)
    {
        var entity = await _subjectRepository
            .Include(x => x.Assignments)
            .FirstOrDefaultAsync(x => x.Id == subjectId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        return _mapper.Map<List<AssignmentDTO>>(entity.Assignments);
    }

    public async Task<AssignmentDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentDTO assignment)
    {
        var entity = await _assignmentRepository.FirstOrDefaultAsync(x => x.Id == assignmentId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {assignmentId}");

        _mapper.Map(assignment, entity);

        await _assignmentRepository.UpdateAsync(entity);

        return _mapper.Map<AssignmentDTO>(entity);
    }

    public async Task<bool> AddMarkForStudent(Guid studentId, Guid assignmentId)
    {
        var studentAnswer = await _studentAnswerRepository
            .Include(x => x.TaskVariant)
            .ThenInclude(x => x.AssignmentTask)
            .Where(x => x.StudentId == studentId && x.TaskVariant.AssignmentTask.AssignmentId == assignmentId)
            .ToListAsync()
            ?? throw new NotFoundException($"Unable to find entity with such key: {studentId} and {assignmentId}");
        
        var mark = studentAnswer.Sum(x => x.Mark);

        var studentAssignment = await _startingTimeRecordRepository
            .FirstOrDefaultAsync(x => x.StudentId == studentId && x.AssignmentId == assignmentId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {studentId} and {assignmentId}");
        
        studentAssignment.Mark = mark;
        
        var result = await _startingTimeRecordRepository.UpdateAsync(studentAssignment);

        return result;
    }
}