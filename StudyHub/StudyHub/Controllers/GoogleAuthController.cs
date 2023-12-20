using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;

namespace StudyHub.Controllers;

[Route("google-auth")]
[ApiController]
public class GoogleAuthController : Controller
{
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> GoogleSignIn([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.GoogleLogin(authorizationCode);
        return Ok(result);
    } 
}
