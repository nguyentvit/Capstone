using Capstone.Domain.ChapterDomain.Models;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            ConfigureChapterTable(builder);
            ConfigureChapterQuestionIdsTable(builder);
        }
        private static void ConfigureChapterTable(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => ChapterId.Of(dbId)
                );

            builder.Property(c => c.Title)
                .HasConversion(
                    title => title.Value,
                    dbTitle => ChapterTitle.Of(dbTitle)
                );

            builder.Property(c => c.Order)
                .HasConversion(
                    order => order.Value,
                    dbOrder => ChapterOrder.Of(dbOrder)
                );

            builder.Property(c => c.SubjectId)
                .HasConversion(
                    subjectId => subjectId.Value,
                    dbSubjectId => SubjectId.Of(dbSubjectId)
                );
        }
        private static void ConfigureChapterQuestionIdsTable(EntityTypeBuilder<Chapter> builder)
        {
            builder.OwnsMany(c => c.QuestionIds, qis =>
            {
                qis.ToTable("ChapterQuestionIds");

                qis.WithOwner().HasForeignKey("ChapterId");

                qis.HasKey("Id");

                qis.Property(q => q.Value)
                    .HasColumnName("QuestionId")
                    .ValueGeneratedNever();
            });

            builder.Metadata.FindNavigation(nameof(Chapter.QuestionIds))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
