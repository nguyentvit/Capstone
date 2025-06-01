namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record QuestionContent
    {
        public string Value { get; }
        private QuestionContent(string value) => Value = value;
        public static QuestionContent Of(string value)
        {
            return new QuestionContent(value);
        }
    }
}
