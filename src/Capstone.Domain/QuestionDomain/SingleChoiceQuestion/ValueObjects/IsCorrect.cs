namespace Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects
{
    public record IsCorrect
    {
        public bool Value { get; }
        private IsCorrect(bool value) => Value = value;
        public static IsCorrect Of(bool value)
        {
            return new IsCorrect(value);
        }
    }
}
