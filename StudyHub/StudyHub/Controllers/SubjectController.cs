using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.Subject;
using StudyHub.Common.Requests;
using System;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubjectController : Controller
{
    private readonly ISubjectService _subjectService;
    private readonly IAssignmentService _assignmentService;

    public SubjectController(
        ISubjectService subjectService,
        IAssignmentService assignmentService)
    {
        _subjectService = subjectService;
        _assignmentService = assignmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> InsertSubject(CreateSubjectDTO dto)
    {
        var userId = HttpContext.GetUserId();
        var result = await _subjectService.AddSubjectAsync(userId, dto);
        return Ok(result);
    }

    [HttpPut("{subjectId}")]
    [Authorize]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateSubject(Guid subjectId, UpdateSubjectDTO dto)
    {
        var userId = HttpContext.GetUserId();
        var result = await _subjectService.UpdateSubjectAsync(userId, subjectId, dto);
        return Ok(result);
    }

    [HttpDelete("{subjectId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteSubject(Guid subjectId)
    {
        var userId = HttpContext.GetUserId();
        return await _subjectService.DeleteSubjectAsync(userId, subjectId) ? NoContent() : NotFound();
    }

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetSubject(Guid subjectId)
    {
        var result = await _subjectService.GetSubjectAsync(subjectId);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetSubjectForUser()
    {
        var studentId = HttpContext.GetUserId();
        var result = await _subjectService.GetSubjectsForUserAsync(studentId);
        return Ok(result);
    }

    [HttpGet("{subjectId}/assignments")]
    public async Task<IActionResult> GetAssignmentBySubjectId(Guid subjectId)
    {
        var result = await _assignmentService.GetAssignmentsBySubjectIdAsync(subjectId);
        return Ok(result);
    }

    [HttpPost("{subjectId}/students")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> AddStudentsToSubject(Guid subjectId, StudentsToSubjectRequest request)
    {
        var teacherId = HttpContext.GetUserId();
        var result = await _subjectService.AddStudentsToSubjectAsync(subjectId, teacherId, request);
        return result.Success != null ? Ok(result) : BadRequest("An error occurred while adding students to the subject.");
    }

    [HttpDelete("{subjectId}/students")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteStudentsFromSubject(Guid subjectId, StudentsToSubjectRequest request)
    {
        var teacherId = HttpContext.GetUserId();
        var result = await _subjectService.DeleteStudentsFromSubjectAsync(subjectId, teacherId, request);
        return result.Success != null ? Ok(result) : BadRequest("An error occurred while adding students to the subject.");
    }

    [HttpGet("{subjectId}/students")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetStudentsForSubject(Guid subjectId)
    {
        var result = await _subjectService.GetStudentsForSubjectAsync(subjectId);
        return Ok(result);
    }
}