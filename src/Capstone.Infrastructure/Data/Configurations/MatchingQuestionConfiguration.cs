using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class MatchingQuestionConfiguration : IEntityTypeConfiguration<MatchingQuestion>
    {
        public void Configure(EntityTypeBuilder<MatchingQuestion> builder)
        {
            builder.HasBaseType<Question>();
            ConfigureMatchingQuestionTable(builder);
            ConfigureMatchingQuestionMatchingPairTable(builder);
        }
        private static void ConfigureMatchingQuestionTable(EntityTypeBuilder<MatchingQuestion> builder)
        {

        }
        private static void ConfigureMatchingQuestionMatchingPairTable(EntityTypeBuilder<MatchingQuestion> builder)
        {
            builder.OwnsMany(mq => mq.MatchingPairs, mps =>
            {
                mps.ToTable("MatchingQuestionsMatchingPairs");

                mps.WithOwner().HasForeignKey("QuestionId");

                mps.HasKey("Id", "QuestionId");

                mps.Property(m => m.Id)
                    .HasConversion(
                        id => id.Value,
                        dbId => MatchingPairId.Of(dbId)
                    );

                mps.OwnsOne(m => m.Right, right =>
                {
                    right.Property(r => r.Value).HasColumnName("RightValue");
                    right.Property(r => r.Id).HasColumnName("RightId");
                });

                mps.OwnsOne(m => m.Left, right =>
                {
                    right.Property(r => r.Value).HasColumnName("LeftValue");
                    right.Property(r => r.Id).HasColumnName("LeftId");
                });

                mps.Property<QuestionId>("QuestionId");
            });

            builder.Metadata.FindNavigation(nameof(MatchingQuestion.MatchingPairs))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
