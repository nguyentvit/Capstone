namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ExamSessionId
    {
        public Guid Value { get; }
        private ExamSessionId(Guid value) => Value = value;
        public static ExamSessionId Of(Guid value)
        {
            return new ExamSessionId(value);
        }
    }
}
