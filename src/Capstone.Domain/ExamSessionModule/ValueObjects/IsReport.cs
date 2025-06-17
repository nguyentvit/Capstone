namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsReport
    {
        public bool Value { get; private set; }
        private IsReport(bool value) => Value = value;
        public static IsReport Of(bool value)
        {
            return new IsReport(value);
        }
    }
}
