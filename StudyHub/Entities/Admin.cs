using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyHub.Entities;

public class Admin
{
    [Key, ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
