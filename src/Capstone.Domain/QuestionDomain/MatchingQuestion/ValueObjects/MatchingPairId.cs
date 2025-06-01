namespace Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects
{
    public record MatchingPairId
    {
        public Guid Value { get; }
        private MatchingPairId(Guid value) => Value = value;
        public static MatchingPairId Of(Guid value)
        {
            return new MatchingPairId(value);
        }
    }
}
