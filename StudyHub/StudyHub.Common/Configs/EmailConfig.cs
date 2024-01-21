namespace StudyHub.Common.Configs;

public class EmailConfig : ConfigBase
{
    public string AcceptInvitationUrl { get; set; } = string.Empty;

    public string MessagePath { get; set; } = string.Empty;

}