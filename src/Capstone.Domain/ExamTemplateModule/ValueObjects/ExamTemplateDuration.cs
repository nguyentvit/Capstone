namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record ExamTemplateDuration
    {
        public int Value { get; }
        private ExamTemplateDuration(int value) => Value = value;
        public static ExamTemplateDuration Of(int value)
        {
            return new ExamTemplateDuration(value);
        }
    }
}
