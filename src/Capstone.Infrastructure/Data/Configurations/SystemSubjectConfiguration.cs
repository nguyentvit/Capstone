using Capstone.Domain.SubjectDomain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class SystemSubjectConfiguration : IEntityTypeConfiguration<SystemSubject>
    {
        public void Configure(EntityTypeBuilder<SystemSubject> builder)
        {
            builder.HasBaseType<Subject>();
            ConfigureSystemSubjectTable(builder);
        }
        private static void ConfigureSystemSubjectTable(EntityTypeBuilder<SystemSubject> builder)
        {

        }
    }
}
