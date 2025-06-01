namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record IsPersonal
    {
        public bool Value { get; }
        private IsPersonal(bool value) => Value = value;
        public static IsPersonal Of(bool value)
        {
            return new IsPersonal(value);
        }
    }
}
