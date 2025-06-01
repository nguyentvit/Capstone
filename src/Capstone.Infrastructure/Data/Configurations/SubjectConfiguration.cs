using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.SubjectDomain.Models;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            ConfigureSubjectTable(builder);
            ConfigureSubjectQuestionIdsTable(builder);
        }
        private static void ConfigureSubjectTable(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => SubjectId.Of(dbId)
                );

            builder.Property(s => s.SubjectName)
                .HasConversion(
                    name => name.Value,
                    dbName => SubjectName.Of(dbName)
                );

            builder.Property(s => s.IsDeleted)
                .HasConversion(
                    isDeleted => isDeleted.Value,
                    dbIsDeleted => IsDeleted.Of(dbIsDeleted)
                );
        }
        private static void ConfigureSubjectQuestionIdsTable(EntityTypeBuilder<Subject> builder)
        {
            builder.OwnsMany(s => s.ChapterIds, cis =>
            {
                cis.ToTable("SubjectChapterIds");

                cis.WithOwner().HasForeignKey("SubjectId");

                cis.HasKey("Id");

                cis.Property(q => q.Value)
                    .HasColumnName("ChapterId")
                    .ValueGeneratedNever();
            });

            builder.Metadata.FindNavigation(nameof(Subject.ChapterIds))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
