using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class StudentSelectedOption : EntityBase
{
    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }

    [ForeignKey(nameof(Option))]
    public Guid OptionId { get; set; }

    public AssignmentTaskOptionBase Option { get; set; } = null!;
    public User Student { get; set; } = null!;
}