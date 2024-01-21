using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyHub.Entities;

namespace StudyHub.DAL.EF;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<AssignmentTask> AssignmentTasks { get; set; } = null!;
    public DbSet<InvitedUser> InvitedUsers { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<StartingTimeRecord> StartingTimeRecords { get; set; } = null!;
    public DbSet<StudentAnswer> StudentAnswers { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<TaskOption> TaskOptions { get; set; } = null!;
    public DbSet<TaskVariant> TaskVariants { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>()
            .HasMany(subject => subject.Students)
            .WithMany(user => user.Subjects);

        modelBuilder.Entity<Subject>()
            .HasOne(subject => subject.Teacher)
            .WithMany(user => user.TeacherSubjects);

        modelBuilder.Entity<TaskOption>()
            .HasOne(variant => variant.TaskVariantChoiceOption)
            .WithMany(options => options.ChoiceOption);

        modelBuilder.Entity<TaskOption>()
           .HasOne(variant => variant.TaskVariantOpenEnded)
           .WithMany(options => options.OpenEndedOption);

        base.OnModelCreating(modelBuilder);
    }
}