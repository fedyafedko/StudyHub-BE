using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.BLL.Services.Interfaces;
public interface IAuthService
{
    Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user);
    Task<AuthSuccessDTO> RefreshTokenAsync(string token, string refreshToken);
    Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user);
}
