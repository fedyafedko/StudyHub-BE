using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;
public class Subject : EntityBase
{
    [ForeignKey(nameof(User))]
    public Guid TeacherId { get; set; }
    public string Title { get; set; } = string.Empty;
    public User User { get; set; } = null!;
}
