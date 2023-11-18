namespace StudyHub.Entities;

public class Student
{
    public Guid UserId { get; set; }
    public string Group { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public List<Subject> Subjects { get; set; } = null!;
    public List<StudentSelectedOption> SelectedOptions { get; set; } = null!;
}