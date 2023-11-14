namespace StudyHub.Entities;

public class Teacher : User
{
    public string Telegram { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = null!;
}
