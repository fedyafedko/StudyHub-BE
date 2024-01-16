namespace StudyHub.BLL.Extensions;

public static class EncryptExtension
{
    public static string Encrypt(this string token)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(token);
    }

    public static bool Descrypt(this string token, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(token, hash);
    }
}
