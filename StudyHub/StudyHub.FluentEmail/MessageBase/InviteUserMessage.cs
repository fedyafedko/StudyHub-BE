namespace StudyHub.FluentEmail.MessageBase;

public class InviteUserMessage : EmailMessageBase
{
    public override string Subject => "Invitation";
    public override string TemplateName => nameof(InviteUserMessage);
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string InviteUserUrl { get; set; } = string.Empty;
}
