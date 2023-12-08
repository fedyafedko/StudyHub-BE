using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;

namespace StudyHub.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInvitedController : Controller
{
    private readonly IUserInvitedService _adminService;
    public UserInvitedController(IUserInvitedService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("Teacher")]
    [Authorize(Roles = "Teacher, Admin")]
    public async Task<IActionResult> CreateRegistrationUrlAsync(string email)
    {
        var role = User.IsInRole("Teacher") switch
        {
            true => "Teacher",
            false => "Admin"
        };

        await _adminService.CreateRegistrationUrl(email, role);
        return Ok();
    }
}
