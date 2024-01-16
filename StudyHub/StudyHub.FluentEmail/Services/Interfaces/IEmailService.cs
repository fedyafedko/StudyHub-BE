using StudyHub.FluentEmail.MessageBase;

namespace StudyHub.FluentEmail.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync<T>(string to, T message)
        where T : EmailMessageBase;

    Task SendManyAsync<T>(List<T> message)
        where T : EmailMessageBase;
}