namespace Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects
{
    public record ChoiceSingleId
    {
        public Guid Value { get; }
        private ChoiceSingleId(Guid value) => Value = value;
        public static ChoiceSingleId Of(Guid value)
        {
            return new ChoiceSingleId(value);
        }
    }
}
