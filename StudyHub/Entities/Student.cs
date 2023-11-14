namespace StudyHub.Entities;

public class Student : User
{
    public string Group { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = null!;
    public List<StudentSelectedOption> SelectedOptions { get; set; } = null!;
}