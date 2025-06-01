namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record FullName
    {
        public string Value { get; }
        private FullName(string value) => Value = value;
        public static FullName Of(string value)
        {
            return new FullName(value);
        }
    }
}
