using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.Assignment;
using StudyHub.Common.DTO.User.Student;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssignmentController : Controller
{
    private readonly IAssignmentService _assignmentService;
    private readonly IStudentAnswerService _studentAnswerService;

    public AssignmentController(
        IAssignmentService assignmentService,
        IStudentAnswerService studentAnswerService)
    {
        _assignmentService = assignmentService;
        _studentAnswerService = studentAnswerService;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> InsertAssignment(CreateAssignmentDTO dto)
    {
        var result = await _assignmentService.CreateAssignmentAsync(dto);
        return Ok(result);
    }

    [HttpPut("{assignmentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateAssignment(Guid assignmentId, UpdateAssignmentDTO dto)
    {
        var result = await _assignmentService.UpdateAssignmentAsync(assignmentId, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
    {
        return await _assignmentService.DeleteAssignmentAsync(assignmentId) ? NoContent() : NotFound();
    }

    [HttpGet("{assignmentId}")]
    public async Task<IActionResult> GetAssignment(Guid assignmentId)
    {
        var result = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
        return Ok(result);
    }

    [HttpPut("student-answer")]
    public async Task<IActionResult> UpsertStudentAnswer(StudentAnswerDTO dto)
    {
        var studentId = HttpContext.GetUserId();

        await _studentAnswerService.UpsertStudentAnswerAsync(studentId, dto);

        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> NextAssignment()
    {
        var studentId = HttpContext.GetUserId();

        var result = await _assignmentService.GetNextAssignmentAsync(studentId);

        return Ok(result);
    }
}