using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Requests;
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

    public async Task<SubjectDTO> AddSubjectAsync(Guid teacherId, CreateSubjectDTO dto)
    {
        var teacher = await _userManager.FindByIdAsync(teacherId)
            ?? throw new NotFoundException($"Teacher not found in the database with this ID: {teacherId}");

        var entity = _mapper.Map<Subject>(dto);

        entity.Teacher = teacher;

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

    public async Task<List<SubjectDTO>> GetSubjectsForUserAsync(Guid userId)
    {
        var user = await _userManager.Users
            .Include(u => u.TeacherSubjects)
            .Include(u => u.Subjects)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new NotFoundException($"Unable to find entity with such key: {userId}");

        var roles = await _userManager.GetRolesAsync(user);

        var subjects = roles.First() == "Student" 
            ? user.Subjects
            : user.TeacherSubjects;

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

    public async Task<List<StudentDTO>> AddStudentsToSubjectAsync(
        Guid subjectId,
        Guid teacherId,
        StudentsToSubjectRequest request)
    {
        var subject = await _subjectRepository.FirstOrDefaultAsync(s => s.Id == subjectId)
            ?? throw new NotFoundException("Subject not found with the specified Id");

        if (subject.TeacherId != teacherId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        foreach (var email in request.Emails)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"Student not found with the specified email: {email}");

            user.Subjects ??= new List<Subject>();

            user.Subjects.Add(subject);

            await _userManager.UpdateAsync(user);
        }

        return _mapper.Map<List<StudentDTO>>(subject.Students);
    }

    public async Task<bool> DeleteStudentsSubjectAsync(
        Guid subjectId,
        Guid teacherId,
        StudentsToSubjectRequest request)
    {
        var subject = await _subjectRepository
                .Include(s => s.Students)
                .FirstOrDefaultAsync(s => s.Id == subjectId)
            ?? throw new NotFoundException("Subject not found with the specified Id");

        if (subject.TeacherId != teacherId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        foreach (var email in request.Emails)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"Student not found with the specified email: {email}");

            subject.Students.Remove(user);

            await _subjectRepository.UpdateAsync(subject);
        }

        return true;
    }

    public async Task<List<StudentDTO>> GetStudentsForSubjectAsync(Guid subjectId)
    {
        var subject = await _subjectRepository
                .Include(s => s.Students)
                .FirstOrDefaultAsync(s => s.Id == subjectId)
            ?? throw new NotFoundException("Subject not found with the specified Id");

        return _mapper.Map<List<StudentDTO>>(subject.Students);
    }
}