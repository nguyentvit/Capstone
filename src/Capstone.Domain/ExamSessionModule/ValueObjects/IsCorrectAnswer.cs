namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsCorrectAnswer
    {
        public bool Value { get; set; }
        private IsCorrectAnswer(bool value) => Value = value;
        public static IsCorrectAnswer Of(bool value)
        {
            return new IsCorrectAnswer(value);
        }
    }
}
