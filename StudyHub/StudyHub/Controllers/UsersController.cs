using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.Requests;

namespace StudyHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IUserInvitationService _userInvitingService;
    private readonly IGetStudentService _getStudentService;

    public UsersController(IUserInvitationService userInvitingService, IGetStudentService getStudentService)
    {
        _userInvitingService = userInvitingService;
        _getStudentService = getStudentService;
    }

    [HttpPost("invite")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> InviteUsers(InviteUsersRequest dto)
    {
        var userId = HttpContext.GetUserId();

        return await _userInvitingService.InviteManyAsync(userId, dto) ? NoContent() : BadRequest();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> SearchStudent([FromQuery] SearchRequest request)
    {
        var result = await _getStudentService.GetStudents(request);
        return Ok(result);
    }
}