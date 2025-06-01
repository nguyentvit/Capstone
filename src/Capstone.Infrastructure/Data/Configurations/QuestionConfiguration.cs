using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            ConfigureQuestionTable(builder);
        }
        private static void ConfigureQuestionTable(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasConversion(
                    questionId => questionId.Value,
                    dbQuestionId => QuestionId.Of(dbQuestionId)
                );

            builder.Property(q => q.Title)
                .HasConversion(
                    title => title.Value,
                    dbTitle => QuestionTitle.Of(dbTitle)
                );

            builder.Property(q => q.Content)
                .HasConversion(
                    content => content.Value,
                    dbContent => QuestionContent.Of(dbContent)
                );

            builder.Property(q => q.IsDeleted)
                .HasConversion(
                    isDeleted => isDeleted.Value,
                    dbIsDeleted => IsDeleted.Of(dbIsDeleted)
                );

            builder.Property(q => q.IsActive)
                .HasConversion(
                    isActive => isActive.Value,
                    dbIsActive => IsActive.Of(dbIsActive)
                );

            builder.Property(q => q.Difficulty)
                .HasConversion(
                    difficulty => difficulty.ToString(),
                    dbDifficulty => (QuestionDifficulty)Enum.Parse(typeof(QuestionDifficulty), dbDifficulty)
                );

            builder.Property(q => q.Type)
                .HasConversion(
                    type => type.ToString(),
                    dbType => (QuestionType)Enum.Parse(typeof(QuestionType), dbType)
                );

            builder.Property(q => q.IsPersonal)
                .HasConversion(
                    isPersonal => isPersonal.Value,
                    dbIsPersonal => IsPersonal.Of(dbIsPersonal)
                );

            builder.Property(q => q.UserId)
                .HasConversion(
                    userId => userId.Value,
                    dbUserId => UserId.Of(dbUserId)
                );

            var chapterIdConverter = new ValueConverter<ChapterId?, Guid?>(
                chapterId => chapterId != null ? chapterId.Value : (Guid?)null,
                dbChapterId => dbChapterId != null ? ChapterId.Of(dbChapterId.Value) : null
            );

            builder.Property(q => q.ChapterId)
                   .HasConversion(chapterIdConverter);

            var beforeQuestionId = new ValueConverter<QuestionId?, Guid?>(
                beforeQuestionId => beforeQuestionId != null ? beforeQuestionId.Value : (Guid?)null,
                dbBeforeQuestionid => dbBeforeQuestionid != null ? QuestionId.Of(dbBeforeQuestionid.Value) : null
                );

            builder.Property(q => q.BeforeQuestionId)
                   .HasConversion(beforeQuestionId);

            builder.Property(q => q.RootId)
                .HasConversion(
                    rootId => rootId.Value,
                    dbRootId => QuestionId.Of(dbRootId)
                );

            builder.Property(q => q.IsLastVersion)
                .HasConversion(
                    isLastVersion => isLastVersion.Value,
                    dbIsLastVersion => IsLastVersion.Of(dbIsLastVersion)
                );

        }
    }
}
    