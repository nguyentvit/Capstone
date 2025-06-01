namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ExamSessionName
    {
        public string Value { get; }
        private ExamSessionName(string value) => Value = value;
        public static ExamSessionName Of(string value)
        {
            return new ExamSessionName(value);
        }
    }
}
