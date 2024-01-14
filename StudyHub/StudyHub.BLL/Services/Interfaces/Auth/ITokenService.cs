using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces.Auth;
public interface ITokenService
{
    Task<string> GenerateJwtTokenAsync(User user);
    string GenerateRefreshTokenAsync(User user);
}
