namespace Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects
{
    public record ChoiceSingleContent
    {
        public string Value { get; }
        private ChoiceSingleContent(string value) => Value = value;
        public static ChoiceSingleContent Of(string value)
        {
            return new ChoiceSingleContent(value);
        }
    }
}
