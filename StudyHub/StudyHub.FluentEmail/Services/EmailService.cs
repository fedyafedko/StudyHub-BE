using FluentEmail.Core;
using Microsoft.Extensions.Options;
using StudyHub.Common.Models;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;

namespace StudyHub.FluentEmail.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;
    private readonly EmailSettings _messageSettings;

    public EmailService(IFluentEmail fluentEmail, IOptions<EmailSettings> messageSettings, IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmail = fluentEmail;
        _messageSettings = messageSettings.Value;
        _fluentEmailFactory = fluentEmailFactory;
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

        return sendEmail.Successful;
    }

    public async Task<bool> SendManyAsync<T>(List<T> message)
        where T : EmailMessageBase
    {
        foreach (var item in message)
        {
            var path = $@"{Directory.GetCurrentDirectory()}{_messageSettings.MessagePath}\{item.TemplateName}.cshtml";
            var sendEmail = await _fluentEmailFactory
                 .Create()
                 .To(item.Recipient)
                 .Subject(item.Subject)
                 .UsingTemplateFromFile(path, item)
                 .SendAsync();

            if (!sendEmail.Successful)
                return false;
        }

        return true;
    }
}