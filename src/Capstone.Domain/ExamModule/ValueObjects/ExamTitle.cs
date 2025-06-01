namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamTitle
    {
        public string Value { get; }
        private ExamTitle(string value) => Value = value;
        public static ExamTitle Of(string value)
        {
            return new ExamTitle(value);
        }
    }
}
