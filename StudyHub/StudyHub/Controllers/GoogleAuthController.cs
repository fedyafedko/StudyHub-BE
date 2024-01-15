using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces.Auth;

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

    [HttpPost("[action]")]
    public async Task<IActionResult> GoogleSignUp([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.GoogleRegisterAsync(authorizationCode);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GoogleSignIn([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.GoogleLoginAsync(authorizationCode);
        return Ok(result);
    } 
}
