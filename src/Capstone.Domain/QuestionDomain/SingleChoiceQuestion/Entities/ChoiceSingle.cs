using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Entities
{
    public class ChoiceSingle : Entity<ChoiceSingleId>
    {
        public ChoiceSingleContent Content { get; private set; } = default!;
        private ChoiceSingle() { }
        private ChoiceSingle(ChoiceSingleContent content)
        {
            Id = ChoiceSingleId.Of(Guid.NewGuid());
            Content = content;
        }
        public static ChoiceSingle Of(ChoiceSingleContent content)
        {
            return new ChoiceSingle(content);
        }
    }
}
