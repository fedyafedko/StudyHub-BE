using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO;

namespace StudyHub.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserDTO userDTO)
    {
        var result = await _authService.RegisterAsync(userDTO);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserDTO userDTO)
    {
        var result = await _authService.LoginAsync(userDTO);
        return Ok(result);
    }
}
