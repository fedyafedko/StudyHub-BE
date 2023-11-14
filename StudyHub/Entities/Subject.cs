using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;
public class Subject
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Teacher Teacher { get; set; } = null!;
}
