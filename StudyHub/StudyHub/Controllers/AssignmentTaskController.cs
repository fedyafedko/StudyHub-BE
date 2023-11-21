using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;
using StudyHub.Validators;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
public class AssignmentTaskController : Controller
{
    private readonly IAssignmentTaskService _serviceTask;
    private readonly AssignmentTaskValidator _assignmentTaskValidator;

    public AssignmentTaskController(
        IAssignmentTaskService serviceTask,
        AssignmentTaskValidator assignmentTaskValidator)
    {
        _serviceTask = serviceTask;
        _assignmentTaskValidator = assignmentTaskValidator;
    }

    [HttpPost]
    public async Task<IActionResult> InsertAssigmentTask(CreateAssignmentTaskDTO dto)
    {
        _assignmentTaskValidator.ValidateAndThrow(dto);

        var result = await _serviceTask.AddTask(dto);
        return Ok(result);
    }
}
