﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyHub.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? Telegram { get; set; } = null;
    public string? Group { get; set; } = null;
    public string? Course { get; set; } = null;
    public string? Faculty { get; set; } = null;

    [ForeignKey(nameof(RefreshToken))]
    public Guid? RefreshTokenId { get; set; }

    public List<Subject> TeacherSubjects { get; set; } = null!;
    public List<StudentSubject> StudentSubjects { get; set; } = null!;
    public List<StudentAnswer> Answers { get; set; } = null!;
    public RefreshToken? RefreshToken { get; set; }
}