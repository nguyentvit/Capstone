namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsFree
    {
        public bool Value { get; }
        private IsFree(bool value) => Value = value;
        public static IsFree Of(bool value)
        {
            return new IsFree(value);
        }
    }
}
