using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.BLL.Services.Interfaces;
public interface IGoogleAuthService
{
    Task<AuthSuccessDTO> GoogleLogin(string oauthToken);
}
