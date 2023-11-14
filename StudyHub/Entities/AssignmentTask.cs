using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class AssignmentTask : EntityBase
{
    [ForeignKey(nameof(Assignment))]
    public Guid AssignmentId { get; set; }

    public string Lable { get; set; } = string.Empty;
    public int Mark { get; set; }
    public Assignment Assignment { get; set; } = null!;
}
