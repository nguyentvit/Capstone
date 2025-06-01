namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamVersionId
    {
        public Guid Value { get; }
        private ExamVersionId(Guid value) => Value = value;
        public static ExamVersionId Of(Guid value)
        {
            return new ExamVersionId(value);
        }
    }
}
