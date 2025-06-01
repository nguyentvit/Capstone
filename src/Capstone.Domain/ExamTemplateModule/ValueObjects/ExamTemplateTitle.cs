namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record ExamTemplateTitle
    {
        public string Value { get; }
        private ExamTemplateTitle(string value) => Value = value;
        public static ExamTemplateTitle Of(string value)
        {
            return new ExamTemplateTitle(value);
        }
    }
}
