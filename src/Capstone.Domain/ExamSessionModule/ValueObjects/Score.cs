namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record Score
    {
        public double Value { get; }
        private Score(double value) => Value = value;
        public static Score Of(double value)
        {
            return new Score(value);
        }
    }
}
