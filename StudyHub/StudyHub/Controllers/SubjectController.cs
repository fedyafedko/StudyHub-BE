using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;
using StudyHub.Common.Requests;

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

    [HttpPost("[action]")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> AddStudents(Guid subjectId, AddStudentsToSubjectRequest request)
    {
        var teacherId = HttpContext.GetUserId();
        var result = await _subjectService.AddStudentsToSubjectAsync(subjectId, teacherId, request);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> InsertSubject(CreateSubjectDTO dto)
    {
        var result = await _subjectService.AddSubjectAsync(dto);
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
}