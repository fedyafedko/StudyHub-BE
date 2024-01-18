namespace StudyHub.Hangfire;
public class HangfireConfig
{
    public string ClearingUserInvitationCron { get; set; } = string.Empty;
    public TimeSpan LifeTime { get; set; }
}
