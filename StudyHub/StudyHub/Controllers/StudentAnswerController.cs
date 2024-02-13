using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.User.Student;

namespace StudyHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentAnswerController : Controller
{
    private readonly IStudentAnswerService _studentAnswerService;

    public StudentAnswerController(IStudentAnswerService studentAnswerService)
    {
        _studentAnswerService = studentAnswerService;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateStudentAnswer(StudentAnswerDTO dto)
    {
        var studentId = HttpContext.GetUserId();

        await _studentAnswerService.UpsertStudentAnswerAsync(studentId, dto);

        return Ok();
    }
}
