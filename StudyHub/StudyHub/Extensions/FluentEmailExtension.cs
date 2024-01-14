using System.Net.Mail;
using System.Net;

namespace StudyHub.Extensions;

public static class FluentEmailExtension
{
    public static void AddFluentEmail(this IServiceCollection services, ConfigurationManager configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        SmtpClient client = new SmtpClient
        {
            Credentials = new NetworkCredential(emailSettings["DefaultFromEmail"], emailSettings["Password"]),
            Host = emailSettings["SMTPSetting:Host"]!,
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var defaultFromEmail = emailSettings["DefaultFromEmail"];
        services.AddFluentEmail(defaultFromEmail)
            .AddSmtpSender(client)
            .AddRazorRenderer();
    }
}
