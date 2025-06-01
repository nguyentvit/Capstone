namespace Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects
{
    public record ChoiceMultiContent
    {
        public string Value { get; }
        private ChoiceMultiContent(string value) => Value = value;
        public static ChoiceMultiContent Of(string value)
        {
            return new ChoiceMultiContent(value);
        }
    }
}
