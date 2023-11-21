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
    private readonly AssignmentTaskOptionValidator _optionsValidator;

    public AssignmentTaskController(
        IAssignmentTaskService serviceTask,
        AssignmentTaskOptionValidator optionsValidator)
    {
        _serviceTask = serviceTask;
        _optionsValidator = optionsValidator;
    }

    [HttpPost]
    public async Task<IActionResult> InsertAssigmentTask(CreateAssignmentTaskDTO dto)
    {
        _optionsValidator.ValidateAndThrow(dto.Options);

        var result = await _serviceTask.AddTask(dto);
        return Ok(result);
    }
}
