namespace Capstone.Domain.TeacherDomain.ValueObjects
{
    public record TeacherId
    {
        public string Value { get; }
        private TeacherId(string value) => Value = value;
        public static TeacherId Of(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
            return new TeacherId(value);
        }
    }
}
