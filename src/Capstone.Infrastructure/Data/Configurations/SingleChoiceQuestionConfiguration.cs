using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class SingleChoiceQuestionConfiguration : IEntityTypeConfiguration<SingleChoiceQuestion>
    {
        public void Configure(EntityTypeBuilder<SingleChoiceQuestion> builder)
        {
            builder.HasBaseType<Question>();
            ConfigureSingleChoiceQuestionTable(builder);
            ConfigureChoiceTable(builder);
        }
        private static void ConfigureSingleChoiceQuestionTable(EntityTypeBuilder<SingleChoiceQuestion> builder)
        {
            builder.Property(s => s.CorrectAnswerId)
                .HasConversion(
                    correctAnswerId => correctAnswerId.Value,
                    dbCorrectAnswerId => ChoiceSingleId.Of(dbCorrectAnswerId)
                );
        }
        private static void ConfigureChoiceTable(EntityTypeBuilder<SingleChoiceQuestion> builder)
        {
            builder.OwnsMany(s => s.Choices, cs =>
            {
                cs.ToTable("SingleChoiceQuestionsChoices");

                cs.WithOwner().HasForeignKey("QuestionId");

                cs.HasKey("Id", "QuestionId");

                cs.Property(c => c.Id)
                    .HasColumnName("SingleChoiceQuestionChoiceId")
                    .HasConversion(
                        id => id.Value,
                        dbId => ChoiceSingleId.Of(dbId)
                    );

                cs.Property(c => c.Content)
                    .HasConversion(
                        content => content.Value,
                        dbContent => ChoiceSingleContent.Of(dbContent)
                    );

                cs.Property<QuestionId>("QuestionId");
            });

            builder.Metadata.FindNavigation(nameof(SingleChoiceQuestion.Choices))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
