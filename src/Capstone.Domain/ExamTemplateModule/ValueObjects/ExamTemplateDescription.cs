namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record ExamTemplateDescription
    {
        public string Value { get; }
        private ExamTemplateDescription(string value) => Value = value;
        public static ExamTemplateDescription Of(string value)
        {
            return new ExamTemplateDescription(value);
        }
    }
}
