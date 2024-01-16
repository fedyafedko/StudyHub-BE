using StudyHub.FluentEmail.MessageBase;

namespace StudyHub.FluentEmail.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync<T>(string to, T message)
        where T : EmailMessageBase;
}