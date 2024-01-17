namespace StudyHub.BLL.Services.Interfaces.Auth;

public interface IEncryptService
{
    public bool Verify(string token, string hash);
    public string Encrypt(string token);
}
