using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations
{
    public class EssayQuestionConfiguration : IEntityTypeConfiguration<EssayQuestion>
    {
        public void Configure(EntityTypeBuilder<EssayQuestion> builder)
        {
            builder.HasBaseType<Question>();
        }
    }
}
