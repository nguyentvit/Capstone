namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsDone
    {
        public bool Value { get; }
        private IsDone(bool value) => Value = value;
        public static IsDone Of(bool value)
        {
            return new IsDone(value);
        }
    }
}
