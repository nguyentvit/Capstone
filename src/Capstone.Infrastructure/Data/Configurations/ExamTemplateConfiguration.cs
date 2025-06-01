using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.ExamTemplateModule.Models;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class ExamTemplateConfiguration : IEntityTypeConfiguration<ExamTemplate>
    {
        public void Configure(EntityTypeBuilder<ExamTemplate> builder)
        {
            ConfigureExamTemplateTable(builder);
            ConfigureExamTemplateExamTemplateSection(builder);
        }
        private static void ConfigureExamTemplateTable(EntityTypeBuilder<ExamTemplate> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => ExamTemplateId.Of(dbId) 
                );

            builder.Property(e => e.SubjectId)
                .HasConversion(
                    subjectId => subjectId.Value,
                    dbSubjectId => SubjectId.Of(dbSubjectId)
                );

            builder.Property(e => e.Title)
                .HasConversion(
                    title => title.Value,
                    dbTitle => ExamTemplateTitle.Of(dbTitle)
                );

            builder.Property(e => e.Description)
                .HasConversion(
                    description => description.Value,
                    dbDescription => ExamTemplateDescription.Of(dbDescription)
                );

            builder.Property(e => e.DurationInMinutes)
                .HasConversion(
                    duration => duration.Value,
                    dbDuration => ExamTemplateDuration.Of(dbDuration)
                );

            builder.Property(e => e.IsActive)
                .HasConversion(
                    isActive => isActive.Value,
                    dbIsActive => IsActive.Of(dbIsActive)
                );

            builder.Property(e => e.IsDeleted)
                .HasConversion(
                    isDeleted => isDeleted.Value,
                    dbIsDeleted => IsDeleted.Of(dbIsDeleted)
                );
        }
        private static void ConfigureExamTemplateExamTemplateSection(EntityTypeBuilder<ExamTemplate> builder)
        {
            builder.OwnsMany(e => e.ExamTemplateSection, tss =>
            {
                tss.ToTable("ExamTemplateExamTemplateSections");

                tss.WithOwner().HasForeignKey("ExamTemplateId");

                tss.HasKey("Id");

                tss.Property(ts => ts.Id)
                    .HasConversion(
                        id => id.Value,
                        dbId => ExamTemplateSectionId.Of(dbId)
                    );

                tss.Property(ts => ts.ChapterId)
                    .HasConversion(
                        chapterId => chapterId.Value,
                        dbChapterId => ChapterId.Of(dbChapterId)
                    );

                tss.OwnsMany(s => s.DifficultyConfigs, dcs =>
                {
                    dcs.ToTable("ExamTemplateSectionDifficultyConfigs");

                    dcs.WithOwner().HasForeignKey("ExamTemplateSectionId");

                    dcs.Property(dc => dc.Difficulty)
                        .HasConversion<string>();

                    dcs.HasKey("ExamTemplateSectionId", "Difficulty");

                    dcs.OwnsMany(dc => dc.QuestionTypeConfigs, qtc =>
                    {
                        qtc.ToTable("DifficultyConfig_QuestionTypeConfigs");

                        qtc.WithOwner().HasForeignKey("ExamTemplateSectionId", "Difficulty");

                        qtc.Property(q => q.Type).HasConversion<string>();
                        qtc.Property(q => q.NumberOfQuestions);
                        qtc.Property(q => q.PointPerCorrect);
                        qtc.Property(q => q.PointPerInCorrect);

                        qtc.HasKey("ExamTemplateSectionId", "Difficulty", "Type");
                    });

                });
            });

            builder.Metadata.FindNavigation(nameof(ExamTemplate.ExamTemplateSection))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
