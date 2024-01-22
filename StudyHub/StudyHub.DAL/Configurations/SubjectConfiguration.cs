using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyHub.Entities;

namespace StudyHub.DAL.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasMany(subject => subject.Students)
            .WithMany(user => user.Subjects);

        builder.HasOne(subject => subject.Teacher)
            .WithMany(user => user.TeacherSubjects);
    }
}
