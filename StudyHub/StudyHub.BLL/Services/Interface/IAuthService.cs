using StudyHub.Common.DTO;

namespace StudyHub.BLL.Services.Interface;
public interface IAuthService
{
    Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user);
    Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user);
}
