using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AssignmentTask;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
public class AssignmentTaskController : Controller
{
    private readonly IAssignmentTaskService _serviceTask;

    public AssignmentTaskController(IAssignmentTaskService serviceTask)
    {
        _serviceTask = serviceTask;
    }

    [HttpPost]
    public async Task<IActionResult> InsertAssigmentTask(CreateAssignmentTaskDTO dto)
    {
        try
        {
            var result = await _serviceTask.AddTask(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
