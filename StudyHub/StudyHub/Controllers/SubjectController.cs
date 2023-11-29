using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.Subject;

namespace StudyHub.Controllers;

[Route("[controller]")]
[ApiController]
public class SubjectController : Controller
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpPost]
    public async Task<IActionResult> InsertSubject(CreateSubjectDTO dto)
    {
        var result = await _subjectService.AddSubjectAsync(dto);
        return Ok(result);
    }

    [HttpPut("{subjectId}")]
    public async Task<IActionResult> UpdateSubject(Guid subjectId, UpdateSubjectDTO dto)
    {
        var result = await _subjectService.UpdateSubjectAsync(subjectId, dto);
        return Ok(result);
    }

    [HttpDelete("{subjectId}")]
    public async Task<IActionResult> DeleteSubject(Guid subjectId)
    {
        return await _subjectService.DeleteSubjectAsync(subjectId) ? Ok() : NotFound();
    }

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetSubject(Guid subjectId)
    {
        var result = await _subjectService.GetSubjectAsync(subjectId);
        return Ok(result);
    }
}
