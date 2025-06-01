namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record IsLastVersion
    {
        public bool Value { get; private set; }
        private IsLastVersion(bool value) => Value = value;
        public static IsLastVersion Of(bool value)
        {
            return new IsLastVersion(value);
        }
    }
}
