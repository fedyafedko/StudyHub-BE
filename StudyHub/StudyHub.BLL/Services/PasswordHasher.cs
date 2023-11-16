using StudyHub.BLL.Services.Interface;

namespace StudyHub.BLL.Services;
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
