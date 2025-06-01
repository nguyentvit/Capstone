using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Entities
{
    public class ChoiceMulti : Entity<ChoiceMultiId>
    {
        public ChoiceMultiContent Content { get; private set; } = default!;
        public IsCorrect IsCorrect { get; private set; } = default!;
        private ChoiceMulti() { }
        private ChoiceMulti(ChoiceMultiContent content, IsCorrect isCorrect)
        {
            Id = ChoiceMultiId.Of(Guid.NewGuid());
            Content = content;
            IsCorrect = isCorrect;
        }
        public static ChoiceMulti Of(ChoiceMultiContent content, IsCorrect isCorrect)
        {
            return new ChoiceMulti(content, isCorrect);
        }
    }
}
