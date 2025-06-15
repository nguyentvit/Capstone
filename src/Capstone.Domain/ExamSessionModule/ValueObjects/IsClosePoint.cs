namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsClosePoint
    {
        public bool Value { get; private set; }
        private IsClosePoint(bool value) => Value = value;
        public static IsClosePoint Of(bool value)
        {
            return new IsClosePoint(value);
        }
    }
}
