using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
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
    public async Task<IActionResult> InsertSubject(CreateSubjectDTO dto)
    {
        var result = await _subjectService.AddSubjectAsync(dto);
        return Ok(result);
    }
    
    [HttpPut("{subjectId}")]
    [Authorize]
    public async Task<IActionResult> UpdateSubject(Guid subjectId, UpdateSubjectDTO dto)
    {
        var userId = HttpContext.GetUserId();
        var result = await _subjectService.UpdateSubjectAsync(userId, subjectId, dto);
        return Ok(result);
    }

    [HttpDelete("{subjectId}")]
    [Authorize]
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

    [HttpGet("teacher")]
    [Authorize]
    public IActionResult GetSubjectForTeacher()
    {
        var teacherId = HttpContext.GetUserId();
        var result = _subjectService.GetSubjectsForTeacher(teacherId);
        return Ok(result);
    }

    [HttpGet("student")]
    [Authorize]
    public IActionResult GetSubjectForStudent()
    {
        var studentId = HttpContext.GetUserId();
        var result = _subjectService.GetSubjectsForStudent(studentId);
        return Ok(result);
    }

    [HttpGet("{subjectId}/assignments")]
    public async Task<IActionResult> GetAssignmentBySubjectId(Guid subjectId)
    {
        var result = await _assignmentService.GetAssignmentsBySubjectIdAsync(subjectId);
        return Ok(result);
    }
}
