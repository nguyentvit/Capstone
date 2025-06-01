using Capstone.Domain.SubjectDomain.Models;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
    {
        public void Configure(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.HasBaseType<Subject>();
            ConfigureTeacherSubjectTable(builder);
            ConfigureTeacherSubjectClassTable(builder);
        }
        private static void ConfigureTeacherSubjectTable(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.Property(ts => ts.OwnerId)
                .HasConversion(
                    ownerId => ownerId.Value,
                    dbOwnerId => UserId.Of(dbOwnerId)
                );
        }
        private static void ConfigureTeacherSubjectClassTable(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.OwnsMany(ts => ts.ClassIds, tsc =>
            {
                tsc.ToTable("TeacherSubjectClasses");

                tsc.WithOwner().HasForeignKey("TeacherSubjectId");

                tsc.HasKey("Id");

                tsc.Property(t => t.Value)
                    .HasColumnName("ClassId")
                    .ValueGeneratedNever(); 
            });

            builder.Metadata.FindNavigation(nameof(TeacherSubject.ClassIds))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
