namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record ExamTemplateId
    {
        public Guid Value { get; }
        private ExamTemplateId(Guid value) => Value = value;
        public static ExamTemplateId Of(Guid value)
        {
            return new ExamTemplateId(value);
        }
    }
}
