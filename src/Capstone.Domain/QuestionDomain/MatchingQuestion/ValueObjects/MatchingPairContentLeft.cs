namespace Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects
{
    public record MatchingPairContentLeft
    {
        public string Value { get; }
        public Guid Id { get; }
        private MatchingPairContentLeft(string value)
        {
            Value = value;
            Id = Guid.NewGuid();
        }
        public static MatchingPairContentLeft Of(string value)
        {
            return new MatchingPairContentLeft(value);
        }
    }
}
