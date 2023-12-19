using StudyHub.Common.DTO;
using System.Text;

namespace StudyHub.Common.Models;
public class EmailMessageBuilder
{
    public string BuildInvitationEmail(InvitedUserDTO invitedUserDTO)
    {
        string url = $"https://localhost:1234/accept-invitation?role={invitedUserDTO.Role}&token={invitedUserDTO.Token}";

        StringBuilder template = new();
        template.AppendLine($"Dear {invitedUserDTO.Email},");
        template.AppendLine($"You were invited to the course by {invitedUserDTO.Role}, your invitation url: {url}");
        template.AppendLine("Sincerely, our dev team!");

        return template.ToString();
    }
}
