namespace Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects
{
    public record MatchingPairContentRight
    {
        public string Value { get; }
        public Guid Id { get; }
        private MatchingPairContentRight(string value)
        {
            Value = value;
            Id = Guid.NewGuid();
        }
        public static MatchingPairContentRight Of(string value)
        {
            return new MatchingPairContentRight(value);
        }
    }
}
