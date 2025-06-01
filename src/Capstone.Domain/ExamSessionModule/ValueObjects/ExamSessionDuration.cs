namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ExamSessionDuration
    {
        public int Value { get; }
        private ExamSessionDuration(int value) => Value = value;
        public static ExamSessionDuration Of(int value)
        {
            return new ExamSessionDuration(value);
        }
    }
}
