using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class TrueFalseQuestionConfiguration : IEntityTypeConfiguration<TrueFalseQuestion>
    {
        public void Configure(EntityTypeBuilder<TrueFalseQuestion> builder)
        {
            builder.HasBaseType<Question>();
            ConfigureTrueFalseQuestionTable(builder);
        }
        private static void ConfigureTrueFalseQuestionTable(EntityTypeBuilder<TrueFalseQuestion> builder)
        {
            builder.Property(tfq => tfq.IsTrueAnswer)
                .HasConversion(
                    isTrueAnswer => isTrueAnswer.Value,
                    dbIsTrueAnswer => IsTrueAnswer.Of(dbIsTrueAnswer)
                );
        }
    }
}
