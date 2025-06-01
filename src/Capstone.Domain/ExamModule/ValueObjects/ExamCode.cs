namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamCode
    {
        public string Value { get; }
        private ExamCode(string value) => Value = value;
        public static ExamCode Of(string value)
        {
            return new ExamCode(value);
        }
    }
}
