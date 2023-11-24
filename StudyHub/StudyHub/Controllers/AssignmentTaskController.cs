using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Validators;
using System.ComponentModel.DataAnnotations;

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

        var result = await _assignmentTaskService.AddAssignmentTask(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([Required] Guid assignmentId)
    {
        var result = await _assignmentTaskService.GetAssignmentTask(assignmentId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAssigmentTask(Guid assignmentTaskId, UpdateAssignmentTaskDTO dto)
    {
        _updateAssignmentTaskValidator.ValidateAndThrow(dto);

        var result = await _assignmentTaskService.UpdateAssignmentTask(assignmentTaskId, dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAssignmentTask(Guid id)
    {
        return await _assignmentTaskService.DeleteAssignmentTask(id) ? Ok() : NotFound();
    }
}
