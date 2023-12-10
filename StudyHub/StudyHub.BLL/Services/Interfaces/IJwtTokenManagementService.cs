using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;
public interface IJwtTokenManagementService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshTokenAsync(User user);
}
