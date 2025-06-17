using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamSessionModule.Enums;
using Capstone.Domain.ExamSessionModule.Models;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.StudentDomain.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class ExamSessionConfiguration : IEntityTypeConfiguration<ExamSession>
    {
        public void Configure(EntityTypeBuilder<ExamSession> builder)
        {
            ConfigureExamSessionTable(builder);
            ConfigureParticipantExamSessionTable(builder);
        }
        public static void ConfigureExamSessionTable(EntityTypeBuilder<ExamSession> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasConversion(
                    eId => eId.Value,
                    dbId => ExamSessionId.Of(dbId)
                );

            builder.Property(e => e.Name)
                .HasConversion(
                    name => name.Value,
                    dbName => ExamSessionName.Of(dbName)
                );

            builder.Property(e => e.StartTime)
                .HasConversion(
                    start => start.Value,
                    dbStart => Date.Of(dbStart)
                );

            builder.Property(e => e.EndTime)
                .HasConversion(
                    end => end.Value,
                    dbEnd => Date.Of(dbEnd)
                );

            builder.Property(e => e.Duration)
                .HasConversion(
                    duration => duration.Value,
                    dbDuration => ExamSessionDuration.Of(dbDuration)
                );

            builder.Property(e => e.IsCodeBased)
                .HasConversion(
                    isCodeBased => isCodeBased.Value,
                    dbIsCodeBased => IsCodeBased.Of(dbIsCodeBased)
                );

            builder.Property(e => e.ExamId)
                .HasConversion(
                    examId => examId.Value,
                    dbExamId => ExamId.Of(dbExamId)
                );

            var codeConverter = new ValueConverter<ExamSessionCode?, string?>(
                code => code != null ? code.Value : (string?)null,
                dbCode => dbCode != null ? ExamSessionCode.Of(dbCode) : null
            );

            builder.Property(e => e.Code)
                .HasConversion(codeConverter);

            builder.Property(e => e.UserId)
                .HasConversion(
                    userId => userId.Value,
                    dbUserId => UserId.Of(dbUserId)
                );

            var classIdConverter = new ValueConverter<ClassId?, Guid?>(
                    classId => classId != null ? classId.Value : (Guid?)null,
                    dbClassId => dbClassId != null ? ClassId.Of(dbClassId.Value) : null
                    );

            builder.Property(e => e.ClassId)
                .HasConversion(classIdConverter);

            builder.Property(e => e.IsDone)
                .HasConversion(
                    isDone => isDone.Value,
                    dbIsDone => IsDone.Of(dbIsDone)
                );

            builder.Property(e => e.IsClosePoint)
                .HasConversion(
                    isClosePoint => isClosePoint.Value,
                    dbIsClosePoint => IsClosePoint.Of(dbIsClosePoint)
                );

        }
        public static void ConfigureParticipantExamSessionTable(EntityTypeBuilder<ExamSession> builder)
        {
            builder.OwnsMany(e => e.Participants, ps =>
            {
                ps.ToTable("ExamSessionParticipants");

                ps.WithOwner().HasForeignKey("ExamSessionId");

                ps.HasKey(p => p.Id);

                ps.Property(p => p.Id)
                    .HasConversion(
                        id => id.Value,
                        dbId => ParticipantId.Of(dbId)
                    );

                var studentIdConverter = new ValueConverter<StudentId?, string?>(
                    id => id != null ? id.Value : (string?)null,
                    dbId => dbId != null ? StudentId.Of(dbId) : null
                );

                ps.Property(p => p.StudentId)
                    .HasConversion(studentIdConverter);

                var fullNameConverter = new ValueConverter<FullName?, string?>(
                    name => name != null ? name.Value : (string?)null,
                    dbName => dbName != null ? FullName.Of(dbName) : null
                );

                ps.Property(p => p.FullName)
                    .HasConversion(fullNameConverter);

                var emailConverter = new ValueConverter<Email?, string?>(
                    email => email != null ? email.Value : (string?)null,
                    dbEmail => dbEmail != null ? Email.Of(dbEmail) : null
                );

                ps.Property(p => p.Email)
                    .HasConversion(emailConverter);

                ps.Property(p => p.IsFree)
                    .HasConversion(
                        isFree => isFree.Value,
                        dbIsFree => IsFree.Of(dbIsFree)
                    );

                var examVersionIdConverter = new ValueConverter<ExamVersionId?, Guid?>(
                    examVersionId => examVersionId != null ? examVersionId.Value : (Guid?)null,
                    dbExamVersionId => dbExamVersionId != null ? ExamVersionId.Of(dbExamVersionId.Value) : null
                );

                ps.Property(p => p.ExamVersionId)
                    .HasConversion(examVersionIdConverter);

                ps.Property(p => p.IsDone)
                    .HasConversion(
                        isDone => isDone.Value,
                        dbIsDone => IsDone.Of(dbIsDone)
                    );

                var dateConverter = new ValueConverter<Date?, DateTime?>(
                    date => date != null ? date.Value : (DateTime?)null,
                    dbDate => dbDate != null ? Date.Of(dbDate.Value) : null
                    );

                ps.Property(p => p.StartAt)
                    .HasConversion(dateConverter);

                ps.Property(p => p.SubmittedAt)
                    .HasConversion(dateConverter);

                var scoreConverter = new ValueConverter<Score?, double?>(
                    score => score != null ? score.Value : (double?)null,
                    dbScore => dbScore != null ? Score.Of(dbScore.Value) : null
                    );
                    
                ps.Property(p => p.Score)
                    .HasConversion(scoreConverter);

                ps.OwnsMany(p => p.Answers, aws =>
                {
                    aws.ToTable("ParticipantAnswers");

                    aws.WithOwner().HasForeignKey("ParticipantId");

                    aws.HasKey("ParticipantId", "Id");

                    aws.Property(aw => aw.Id)
                        .HasConversion(
                            id => id.Value,
                            dbId => ParticipantAnswerId.Of(dbId)
                        );

                    aws.Property(aw => aw.QuestionId)
                        .HasConversion(
                            questionId => questionId.Value,
                            dbQuestionId => QuestionId.Of(dbQuestionId)
                        );

                    aws.Property(aw => aw.AnswerRaw)
                        .HasConversion(
                            ar => ar.Value,
                            dbAr => AnswerRaw.Of(dbAr)
                        );

                    var scoreConverter = new ValueConverter<Score?, double?>(
                        score => score != null ? score.Value : (double?)null,
                        dbScore => dbScore != null ? Score.Of(dbScore.Value) : null
                        );

                    aws.Property(aw => aw.Score)
                        .HasConversion(scoreConverter);

                    aws.Property(aw => aw.GradingStatus)
                        .HasConversion(
                        status => status.ToString(),
                        dbStatus => (GradingStatus)Enum.Parse(typeof(GradingStatus), dbStatus)
                        );

                    aws.Property(aw => aw.Duration)
                        .HasConversion(
                            duration => duration.Value,
                            dbDuration => Duration.Of(dbDuration)
                        );

                    aws.Property(aw => aw.IsReport)
                        .HasConversion(
                            isReport => isReport.Value,
                            dbIsReport => IsReport.Of(dbIsReport)
                        );

                    aws.Property(aw => aw.IsProcess)
                        .HasConversion(
                            isProcess => isProcess.Value,
                            dbIsProcess => IsProcess.Of(dbIsProcess)
                        );
                });

                ps.OwnsMany(p => p.Actions, ats =>
                {
                    ats.ToTable("ParticipantActions");

                    ats.WithOwner().HasForeignKey("ParticipantId");

                    ats.HasKey("ParticipantId", "Id");

                    ats.Property(at => at.Id)
                        .HasConversion(
                            id => id.Value,
                            dbId => ParticipantActionId.Of(dbId)
                        );

                    ats.Property(at => at.ActionType)
                        .HasConversion(
                            action => action.ToString(),
                            dbAction => (ActionType)Enum.Parse(typeof(ActionType), dbAction)
                        );
                });
            });

            builder.Metadata.FindNavigation(nameof(ExamSession.Participants))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
