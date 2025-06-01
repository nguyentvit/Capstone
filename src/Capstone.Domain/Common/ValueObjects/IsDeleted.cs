namespace Capstone.Domain.Common.ValueObjects
{
    public record IsDeleted
    {
        public bool Value { get; }
        private IsDeleted(bool value) => Value = value;
        public static IsDeleted Of(bool value)
        {
            return new IsDeleted(value);
        }
    }
}
