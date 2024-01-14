using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyHub.Entities;
using System.Reflection.Metadata;

namespace StudyHub.DAL.EF;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<AssignmentTask> AssignmentTasks { get; set; } = null!;
    public DbSet<ChoiceOption> ChoiceAnswers { get; set; } = null!;
    public DbSet<OpenEndedOption> OpenEndedAnswers { get; set; } = null!;
    public DbSet<StudentSelectedOption> StudentSelectedOptions { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<InvitedUser> InvitedUsers { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>()
            .HasMany(subject => subject.Students)
            .WithMany(user => user.Subjects);

        modelBuilder.Entity<Subject>()
            .HasOne(subject => subject.Teacher)
            .WithMany(user => user.TeacherSubjects);

        base.OnModelCreating(modelBuilder);
    }
}
