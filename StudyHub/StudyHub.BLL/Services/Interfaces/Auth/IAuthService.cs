using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.BLL.Services.Interfaces.Auth;
public interface IAuthService
{
    Task<AuthSuccessDTO> LoginAsync(LoginUserDTO dto);
    Task<AuthSuccessDTO> RefreshTokenAsync(RefreshTokenRequest request);
    Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO dto);
}
