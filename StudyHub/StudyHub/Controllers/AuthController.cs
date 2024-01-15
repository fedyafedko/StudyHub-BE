using Microsoft.AspNetCore.Mvc;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Requests;

namespace StudyHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPasswordService _passwordService;

    public AuthController(IAuthService authService, IPasswordService passwordService)
    {
        _authService = authService;
        _passwordService = passwordService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest dto) 
    {
        var result = await _passwordService.ForgotPasswordAsync(dto);

        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest dto)
    {
        var result = await _passwordService.ResetPasswordAsync(dto);

        return Ok(result);
    }
}
