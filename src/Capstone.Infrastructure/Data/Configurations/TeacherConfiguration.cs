using Capstone.Domain.TeacherDomain.Models;
using Capstone.Domain.TeacherDomain.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasBaseType<User>();
            ConfigureTeacherTable(builder);
        }
        private static void ConfigureTeacherTable(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(t => t.TeacherId)
                .HasConversion(
                    teacherId => teacherId.Value,
                    dbTeacherId => TeacherId.Of(dbTeacherId)
                );
        }
    }
}
