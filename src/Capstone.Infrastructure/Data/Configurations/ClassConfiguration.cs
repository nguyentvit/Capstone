using Capstone.Domain.ClassDomain.Models;
using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            ConfigureClassTable(builder);
            ConfigureClassStudentTable(builder);
        }
        private static void ConfigureClassTable(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => ClassId.Of(dbId)
                );

            builder.Property(c => c.Name)
                .HasConversion(
                    name => name.Value,
                    dbClassName => ClassName.Of(dbClassName)
                );

            builder.Property(c => c.SubjectId)
                .HasConversion(
                    subjectId => subjectId.Value,
                    dbSubjectId => SubjectId.Of(dbSubjectId)
                );
        }
        private static void ConfigureClassStudentTable(EntityTypeBuilder<Class> builder)
        {
            builder.OwnsMany(c => c.Students, cs =>
            {
                cs.ToTable("ClassStudents");

                cs.WithOwner().HasForeignKey("ClassId");

                cs.HasKey("Id", "ClassId");

                cs.Property(c => c.Id)
                    .HasColumnName("ClassStudentId")
                    .HasConversion(
                        id => id.Value,
                        dbId => ClassStudentId.Of(dbId)
                    );

                cs.Property(c => c.StudentId)
                    .HasConversion(
                        studentId => studentId.Value,
                        dbStudentId => UserId.Of(dbStudentId)
                    );
            });

            builder.Metadata.FindNavigation(nameof(Class.Students))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
