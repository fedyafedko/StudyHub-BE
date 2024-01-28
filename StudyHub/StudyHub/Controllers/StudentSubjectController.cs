using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.Requests;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentSubjectController : Controller
{
    private readonly IStudentSubjectService _studentSubjectService;

    public StudentSubjectController(IStudentSubjectService studentSubjectService)
    {
        _studentSubjectService = studentSubjectService;
    }
    
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> AddStudents(Guid subjectId, StudentsToSubjectRequest request)
    {
        var teacherId = HttpContext.GetUserId();
        var result = await _studentSubjectService.AddStudentsToSubjectAsync(subjectId, teacherId, request);
        return Ok(result);
    }
    
    [HttpDelete("{subjectId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteStudents(Guid subjectId, StudentsToSubjectRequest request)
    {
        var teacherId = HttpContext.GetUserId();
        var result = await _studentSubjectService.DeleteStudentsSubjectAsync(subjectId, teacherId, request);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{subjectId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetStudents(Guid subjectId)
    {
        var result = await _studentSubjectService.GetStudentsForSubjectAsync(subjectId);
        return Ok(result);
    }
}
