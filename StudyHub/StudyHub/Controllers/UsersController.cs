using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Requests;

namespace StudyHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : Controller
{
    private readonly IUserInvitationService _userInvitingService;
    private readonly IUserService _userService;

    public UsersController(IUserInvitationService userInvitingService, IUserService userService)
    {
        _userInvitingService = userInvitingService;
        _userService = userService;
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
        var result = await _userService.GetStudents(request);
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
    {
        var userId = HttpContext.GetUserId();
        var result = await _userService.UpdateUserAsync(userId, dto);
        return Ok(result);
    }
}