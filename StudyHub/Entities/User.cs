using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StudyHub.Entities;

public class User : IdentityUser<Guid>
{
    public RefreshToken RefreshToken { get; set; } = null!;
}