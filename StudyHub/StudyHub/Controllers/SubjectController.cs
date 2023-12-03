using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
public class SubjectController : Controller
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
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
        return await _subjectService.DeleteSubjectAsync(userId, subjectId) ? Ok() : NotFound();
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
}
