namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamDuration
    {
        public int Value { get; }
        private ExamDuration(int value) => Value = value;
        public static ExamDuration Of(int value)
        {
            return new ExamDuration(value);
        }
    }
}
