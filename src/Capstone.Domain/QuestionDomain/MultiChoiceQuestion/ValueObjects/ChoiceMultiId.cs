namespace Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects
{
    public record ChoiceMultiId
    {
        public Guid Value { get; }
        private ChoiceMultiId(Guid value) => Value = value;
        public static ChoiceMultiId Of(Guid value)
        {
            return new ChoiceMultiId(value);
        }
    }
}
