namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record ExamTemplateSectionId
    {
        public Guid Value { get; }
        private ExamTemplateSectionId(Guid value) => Value = value;
        public static ExamTemplateSectionId Of(Guid value)
        {
            return new ExamTemplateSectionId(value);
        }
    }
}
