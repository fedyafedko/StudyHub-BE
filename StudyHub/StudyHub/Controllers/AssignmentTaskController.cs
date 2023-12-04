using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Validators.AssignmentTaskValidators;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
public class AssignmentTaskController : Controller
{
    private readonly IAssignmentTaskService _assignmentTaskService;
    private readonly CreateAssignmentTaskValidator _createAssignmentTaskValidator;
    private readonly UpdateAssignmentTaskValidator _updateAssignmentTaskValidator;

    public AssignmentTaskController(
        IAssignmentTaskService serviceTask,
        CreateAssignmentTaskValidator createAssignmentTaskValidator,
        UpdateAssignmentTaskValidator updateAssignmentValidator)
    {
        _assignmentTaskService = serviceTask;
        _createAssignmentTaskValidator = createAssignmentTaskValidator;
        _updateAssignmentTaskValidator = updateAssignmentValidator;
    }

    [HttpPost]
    public async Task<IActionResult> InsertAssigmentTask(CreateAssignmentTaskDTO dto)
    {
        _createAssignmentTaskValidator.ValidateAndThrow(dto);

        var result = await _assignmentTaskService.AddAssignmentTaskAsync(dto);
        return Ok(result);
    }

    [HttpGet("{assignmentId}")]
    public async Task<IActionResult> GetAll(Guid assignmentId)
    {
        var result = await _assignmentTaskService.GetAssignmentTaskAsync(assignmentId);
        return Ok(result);
    }

    [HttpPut("{assignmentTaskId}")]
    public async Task<IActionResult> UpdateAssigmentTask(Guid assignmentTaskId, [FromBody] UpdateAssignmentTaskDTO dto)
    {
        _updateAssignmentTaskValidator.ValidateAndThrow(dto);

        var result = await _assignmentTaskService.UpdateAssignmentTaskAsync(assignmentTaskId, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignmentTask(Guid id)
    {
        return await _assignmentTaskService.DeleteAssignmentTaskAsync(id) ? Ok() : NotFound();
    }
}
