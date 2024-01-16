using FluentEmail.Core;
using Microsoft.Extensions.Options;
using StudyHub.Common.Models;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;

namespace StudyHub.FluentEmail.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailSettings _messageSettings;

    public EmailService(IFluentEmail fluentEmail, IOptions<EmailSettings> messageSettings)
    {
        _fluentEmail = fluentEmail;
        _messageSettings = messageSettings.Value;
    }

    public async Task<bool> SendAsync<T>(string to, T message)
        where T : EmailMessageBase
    {
        var path = $@"{Directory.GetCurrentDirectory()}{_messageSettings.MessagePath}\{message.TemplateName}.cshtml";

        var sendEmail = await _fluentEmail
                  .To(to)
                  .Subject(message.Subject)
                  .UsingTemplateFromFile(path, message)
                  .SendAsync();

        _fluentEmail.Data.ToAddresses.Clear();

        return sendEmail.Successful;
    }
}
