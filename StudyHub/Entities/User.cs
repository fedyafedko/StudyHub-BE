using Microsoft.AspNetCore.Identity;

namespace StudyHub.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? Telegram { get; set; } = null;
    public string? Group { get; set; } = null;
    public string? Course { get; set; } = null;

    public RefreshToken RefreshToken { get; set; } = null!;

    public List<Subject> TeacherSubjects { get; set; } = null!;
    public List<Subject> Subjects { get; set; } = null!;
    public List<StudentSelectedOption> SelectedOptions { get; set; } = null!;

}