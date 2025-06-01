namespace Capstone.Domain.StudentDomain.ValueObjects
{
    public record StudentId
    {
        public string Value { get; }
        private StudentId(string value) => Value = value;
        public static StudentId Of(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
            return new StudentId(value);
        }
    }
}
