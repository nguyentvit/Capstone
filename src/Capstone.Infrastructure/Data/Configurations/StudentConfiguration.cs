using Capstone.Domain.StudentDomain.Models;
using Capstone.Domain.StudentDomain.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasBaseType<User>();
            ConfigureStudentTable(builder);
        }
        private static void ConfigureStudentTable(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.StudentId)
                .HasConversion(
                    studentId => studentId.Value,
                    dbStudentId => StudentId.Of(dbStudentId)
                );
        }
    }
}
