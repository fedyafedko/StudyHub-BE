using AutoMapper;
using Google.Apis.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Requests;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class StudentSubjectService : IStudentSubjectService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Subject> _subjectRepository;
    private readonly IMapper _mapper;

    public StudentSubjectService(
        UserManager<User> userManager,
        IRepository<Subject> subjectRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
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
