using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class SubjectService : ISubjectService
{
    private readonly IRepository<Subject> _subjectRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public SubjectService(
        IRepository<Subject> subjectRepository,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _subjectRepository = subjectRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<SubjectDTO> AddSubjectAsync(CreateSubjectDTO dto)
    {
        var teacher = await _userManager.FindByIdAsync(dto.TeacherId)
            ?? throw new NotFoundException($"Teacher not found in the database with this ID: {dto.TeacherId}");

        var entity = _mapper.Map<Subject>(dto);

        await _subjectRepository.InsertAsync(entity);

        return _mapper.Map<SubjectDTO>(entity);
    }

    public async Task<bool> DeleteSubjectAsync(Guid userId, Guid subjectId)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        if (entity.TeacherId != userId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        return await _subjectRepository.DeleteAsync(entity);
    }

    public async Task<SubjectDTO> GetSubjectAsync(Guid subjectId)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        return _mapper.Map<SubjectDTO>(entity);
    }

    public async Task<List<SubjectDTO>> GetSubjectsForStudentAsync(Guid studentId)
    {
        var student = await _userManager.FindByIdAsync(studentId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {studentId}");
            
        var subjects = _mapper
            .Map<List<SubjectDTO>>(student.Subjects);

        return subjects.ToList();
    }

    public async Task<List<SubjectDTO>> GetSubjectsForTeacherAsync(Guid teacherId)
    {
        var subjects = await _subjectRepository.Where(x => x.TeacherId == teacherId).ToListAsync()
            ?? throw new NotFoundException($"Unable to find entity with such key: {teacherId}");

        return _mapper.Map<List<SubjectDTO>>(subjects);
    }

    public async Task<SubjectDTO> UpdateSubjectAsync(Guid userId, Guid subjectId, UpdateSubjectDTO dto)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {subjectId}");
            
        if (entity.TeacherId != userId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        _mapper.Map(dto, entity);

        await _subjectRepository.UpdateAsync(entity);

        return _mapper.Map<SubjectDTO>(entity);
    }
}