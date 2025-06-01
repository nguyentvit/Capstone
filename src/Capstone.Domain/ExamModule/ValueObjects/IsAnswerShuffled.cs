namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record IsAnswerShuffled
    {
        public bool Value { get; }
        private IsAnswerShuffled(bool value) => Value = value;
        public static IsAnswerShuffled Of(bool value)
        {
            return new IsAnswerShuffled(value);
        }
    }
}
