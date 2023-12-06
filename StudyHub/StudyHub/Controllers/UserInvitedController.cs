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
        if (User.IsInRole("Teacher"))
        {
            var result = await _adminService.CreateRegistrationUrl(email, "Teacher");
            if (result)
                return Ok();
        }
        else if (User.IsInRole("Admin"))
        {
            var result = await _adminService.CreateRegistrationUrl(email, "Admin");
            if (result)
                return Ok();
        }

        return BadRequest();
    }
}
