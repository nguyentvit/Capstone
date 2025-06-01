namespace Capstone.Domain.Common.ValueObjects
{
    public record IsActive
    {
        public bool Value { get; }
        private IsActive(bool value) => Value = value;
        public static IsActive Of(bool value)
        {
            return new IsActive(value);
        }
    }
}
