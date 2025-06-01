using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class MultiChoiceQuestionConfiguration : IEntityTypeConfiguration<MultiChoiceQuestion>
    {
        public void Configure(EntityTypeBuilder<MultiChoiceQuestion> builder)
        {
            builder.HasBaseType<Question>();
            ConfigureMultiChoiceQuestionTable(builder);
            ConfigureMultiChoiceQuestionChoiceTable(builder);
        }
        private static void ConfigureMultiChoiceQuestionTable(EntityTypeBuilder<MultiChoiceQuestion> builder)
        {

        }
        private static void ConfigureMultiChoiceQuestionChoiceTable(EntityTypeBuilder<MultiChoiceQuestion> builder)
        {
            builder.OwnsMany(mc => mc.Choices, cs =>
            {
                cs.ToTable("MultiChoiceQuestionsChoices");

                cs.WithOwner().HasForeignKey("QuestionId");

                cs.HasKey("Id", "QuestionId");

                cs.Property(c => c.Id)
                    .HasConversion(
                        id => id.Value,
                        dbId => ChoiceMultiId.Of(dbId)
                    );

                cs.Property(c => c.Content)
                    .HasConversion(
                        content => content.Value,
                        dbContent => ChoiceMultiContent.Of(dbContent)
                    );

                cs.Property(c => c.IsCorrect)
                    .HasConversion(
                        isCorrect => isCorrect.Value,
                        dbIsCorrect => IsCorrect.Of(dbIsCorrect)
                    );

                cs.Property<QuestionId>("QuestionId");
            });

            builder.Metadata.FindNavigation(nameof(MultiChoiceQuestion.Choices))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
