namespace Capstone.Domain.Common.ValueObjects
{
    public record IsPublic
    {
        public bool Value { get; }
        private IsPublic(bool value) => Value = value;
        public static IsPublic Of(bool value)
        {
            return new IsPublic(value);
        }
    }
}
