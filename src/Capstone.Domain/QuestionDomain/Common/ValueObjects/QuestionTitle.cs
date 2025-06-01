namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record QuestionTitle
    {
        public string Value { get; }
        private QuestionTitle(string value) => Value = value;
        public static QuestionTitle Of(string value)
        {
            return new QuestionTitle(value);
        }
    }
}
