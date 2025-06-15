namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record Duration
    {
        public TimeSpan Value { get; }
        private Duration(TimeSpan value) => Value = value;
        public static Duration Of(TimeSpan value)
        {
            return new Duration(value);
        }

    }
}
