using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;
public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshTokenAsync(User user);
}
