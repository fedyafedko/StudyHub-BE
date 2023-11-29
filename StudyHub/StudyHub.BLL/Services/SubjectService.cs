using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;
public class SubjectService : ISubjectService
{
    private readonly IRepository<Subject> _subjectRepository;
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly IMapper _mapper;

    public SubjectService(
        IRepository<Subject> subjectRepository,
        IRepository<Teacher> teacherRepository,
        IMapper mapper)
    {
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<SubjectDTO> AddSubjectAsync(CreateSubjectDTO dto)
    {
        var entity = _mapper.Map<Subject>(dto);
        var teacher = await _teacherRepository.FirstOrDefaultAsync(assignment => assignment.UserId == dto.TeacherId);

        if (teacher == null)
            throw new NotFoundException($"Teacher not found in the database with this ID: {dto.TeacherId}");

        await _subjectRepository.InsertAsync(entity);

        var result = _mapper.Map<SubjectDTO>(entity);
        return result;

    }

    public async Task<bool> DeleteSubjectAsync(Guid subjectId)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId);

        if (entity == null)
            throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        return await _subjectRepository.DeleteAsync(entity);
    }

    public async Task<SubjectDTO> GetSubjectAsync(Guid subjectId)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId);

        if (entity == null)
            throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        return _mapper.Map<SubjectDTO>(entity);
    }

    public async Task<SubjectDTO> UpdateSubjectAsync(Guid subjectId, UpdateSubjectDTO dto)
    {
        var entity = await _subjectRepository
            .FirstOrDefaultAsync(subject => subject.Id == subjectId);

        if (entity == null)
            throw new NotFoundException($"Unable to find entity with such key: {subjectId}");

        _mapper.Map(dto, entity);

        await _subjectRepository.UpdateAsync(entity);

        return _mapper.Map<SubjectDTO>(entity);
    }
}
