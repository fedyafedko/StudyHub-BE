using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorAPI.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.UserInvitation;

namespace StudyHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IUserInvitationService _userInvitingService;
    public UsersController(IUserInvitationService userInvitingService)
    {
        _userInvitingService = userInvitingService;
    }

    [HttpPost("invite")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> InviteUsers(InviteUsersDTO dto)
    {
        var userId = HttpContext.GetUserId();

        return await _userInvitingService.InviteManyAsync(userId, dto) ? NoContent() : BadRequest();
    }
}
