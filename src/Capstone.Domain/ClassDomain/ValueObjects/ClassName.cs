namespace Capstone.Domain.ClassDomain.ValueObjects
{
    public record ClassName
    {
        public string Value { get; set; }
        private ClassName(string value) => Value = value;
        public static ClassName Of(string value)
        {
            return new ClassName(value);
        }
    }
}
