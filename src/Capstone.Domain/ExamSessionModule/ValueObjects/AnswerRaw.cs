namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record AnswerRaw
    {
        public string Value { get; }
        private AnswerRaw(string value) => Value = value;
        public static AnswerRaw Of(string value)
        {
            return new AnswerRaw(value);
        }
    }
}
