namespace StudyHub.Common.Requests;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
