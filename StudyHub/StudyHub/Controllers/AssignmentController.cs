using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces.Assignment;
using StudyHub.Common.DTO.Assignment;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssignmentController : Controller
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
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
}