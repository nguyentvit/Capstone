using Capstone.Domain.ExamModule.Enums;
using Capstone.Domain.ExamModule.Models;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            ConfigureExamTable(builder);
            ConfigureExamExamVersionTable(builder);
        }
        private static void ConfigureExamTable(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => ExamId.Of(dbId)
                );

            builder.Property(e => e.ExamTemplateId)
                .HasConversion(
                    examTemplateId => examTemplateId.Value,
                    dbExamTemplateId => ExamTemplateId.Of(dbExamTemplateId)
                );

            builder.Property(e => e.Duration)
                .HasConversion(
                    duration => duration.Value,
                    dbDuration => ExamDuration.Of(dbDuration)
                );

            builder.Property(e => e.Title)
                .HasConversion(
                    title => title.Value,
                    dbTitle => ExamTitle.Of(dbTitle)
                );

            builder.Property(e => e.UserId)
                .HasConversion(
                    userId => userId.Value,
                    dbUserId => UserId.Of(dbUserId)
                );
        }
        private static void ConfigureExamExamVersionTable(EntityTypeBuilder<Exam> builder)
        {
            builder.OwnsMany(e => e.ExamVersions, evs =>
            {
                evs.ToTable("ExamExamVersions");

                evs.WithOwner().HasForeignKey("ExamId");

                evs.HasKey("Id");

                evs.Property(ev => ev.Id)
                    .HasConversion(
                        id => id.Value,
                        dbId => ExamVersionId.Of(dbId)
                    );

                evs.Property(ev => ev.IsAnswerShuffle)
                    .HasConversion(
                        IsAnswerShuffle => IsAnswerShuffle.Value,
                        dbIsAnswerShuffle => IsAnswerShuffled.Of(dbIsAnswerShuffle)
                    );

                evs.Property(ev => ev.OrderQuestion)
                    .HasConversion(
                        orderQuestion => orderQuestion.ToString(),
                        dbOrderQuestion => (OrderQuestion)Enum.Parse(typeof(OrderQuestion), dbOrderQuestion)
                    );

                evs.Property(ev => ev.Code)
                    .HasConversion(
                        code => code.Value,
                        dbCode => ExamCode.Of(dbCode)
                    );

                evs.OwnsMany(ev => ev.Questions, qs =>
                {
                    qs.ToTable("ExamVersionQuestions");

                    qs.WithOwner().HasForeignKey("ExamVersionId");

                    qs.HasKey("ExamVersionId", "QuestionId");

                    qs.Property(q => q.QuestionId)
                        .HasConversion(
                            questionId => questionId.Value,
                            dbQuestionId => QuestionId.Of(dbQuestionId)
                        );

                    qs.Property(q => q.Order);

                    qs.Property(q => q.PointPerCorrect);

                    qs.Property(q => q.PointPerInCorrect);
                });
            });

            builder.Metadata.FindNavigation(nameof(Exam.ExamVersions))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
