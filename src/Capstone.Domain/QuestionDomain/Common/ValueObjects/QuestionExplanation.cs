namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record QuestionExplanation
    {
        public string Value { get; }
        private QuestionExplanation(string value) => Value = value;
        public static QuestionExplanation Of(string value)
        {
            return new QuestionExplanation(value);
        }
    }
}
