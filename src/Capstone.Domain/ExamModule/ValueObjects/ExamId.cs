namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamId
    {
        public Guid Value { get; }
        private ExamId(Guid value) => Value = value;
        public static ExamId Of(Guid value)
        {
            return new ExamId(value);
        }
    }
}
