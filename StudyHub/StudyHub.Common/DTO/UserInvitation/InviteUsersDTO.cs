namespace StudyHub.Common.DTO.UserInvitation;

public class InviteUsersDTO
{
    public List<string> Emails { get; set; } = null!;
    public string Role { get; set; } = string.Empty;
}
