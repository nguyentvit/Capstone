using Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects;

namespace Capstone.Domain.QuestionDomain.MatchingQuestion.Entities
{
    public class MatchingPair : Entity<MatchingPairId>
    {
        public MatchingPairContentRight Right { get; private set; } = default!;
        public MatchingPairContentLeft Left { get; private set; } = default!;
        private MatchingPair() { }
        private MatchingPair(MatchingPairContentLeft left, MatchingPairContentRight right)
        {
            Id = MatchingPairId.Of(Guid.NewGuid());
            Right = right;
            Left = left;
        }
        public static MatchingPair Of(MatchingPairContentLeft left, MatchingPairContentRight right)
        {
            return new MatchingPair(left, right);
        }
    }
}
