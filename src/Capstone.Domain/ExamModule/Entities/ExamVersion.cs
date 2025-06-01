using Capstone.Domain.ExamModule.Enums;
using Capstone.Domain.ExamModule.ValueObjects;

namespace Capstone.Domain.ExamModule.Entities
{
    public class ExamVersion : Entity<ExamVersionId>
    {
        private readonly List<ExamQuestion> _questions = new();
        public IReadOnlyList<ExamQuestion> Questions => _questions.AsReadOnly();
        public IsAnswerShuffled IsAnswerShuffle { get; private set; } = default!;
        public OrderQuestion OrderQuestion { get; private set; } = default!;
        public ExamCode Code { get; private set; } = default!;
        private ExamVersion() { }
        private ExamVersion(IsAnswerShuffled isAnswerShuffled, OrderQuestion orderQuestion, ExamCode code, List<ExamQuestion> questions)
        {
            Id = ExamVersionId.Of(Guid.NewGuid());
            IsAnswerShuffle = isAnswerShuffled;
            OrderQuestion = orderQuestion;
            Code = code;
            _questions = questions;
        }
        public static ExamVersion Of(IsAnswerShuffled isAnswerShuffled, OrderQuestion orderQuestion, ExamCode code, List<ExamQuestion> questions)
        {
            return new ExamVersion(
                isAnswerShuffled,
                orderQuestion,
                code,
                questions
                );
        }
    }
}
