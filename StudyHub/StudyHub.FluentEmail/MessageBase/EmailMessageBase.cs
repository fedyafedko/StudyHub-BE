namespace StudyHub.FluentEmail.MessageBase;

public abstract class EmailMessageBase
{
    public abstract string Subject { get; }
    public abstract string TemplateName { get; }
    public string Email { get; set; } = string.Empty;
}