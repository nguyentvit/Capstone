namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsCodeBased
    {
        public bool Value { get; }
        private IsCodeBased(bool value) => Value = value;
        public static IsCodeBased Of(bool value)
        {
            return new IsCodeBased(value);
        }
    }
}
