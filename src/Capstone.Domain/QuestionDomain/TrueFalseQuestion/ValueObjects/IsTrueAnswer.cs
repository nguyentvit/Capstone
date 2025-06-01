namespace Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects
{
    public record IsTrueAnswer
    {
        public bool Value { get; }
        private IsTrueAnswer(bool value) => Value = value;
        public static IsTrueAnswer Of(bool value)
        {
            return new IsTrueAnswer(value);
        }
    }
}
