using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Assignment;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssignmentController : Controller
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost]
    public async Task<IActionResult> InsertAssignment(CreateAssignmentDTO dto)
    {
        var result = await _assignmentService.CreateAssignmentAsync(dto);
        return Ok(result);
    }

    [HttpPut("{assignmentId}")]
    public async Task<IActionResult> UpdateAssignment(Guid assignmentId, UpdateAssignmentDTO dto)
    {
        var result = await _assignmentService.UpdateAssignmentAsync(assignmentId, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(Guid id)
    {
        return await _assignmentService.DeleteAssignmentAsync(id) ? NoContent() : NotFound();
    }

    [HttpGet("{assignmentId}")]
    public async Task<IActionResult> GetAssignment(Guid assignmentId)
    {
        var result = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
        return Ok(result);
    }
}