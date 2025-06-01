namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record QuestionId
    {
        public Guid Value { get; }
        private QuestionId(Guid value) => Value = value;
        public static QuestionId Of(Guid value)
        {
            return new QuestionId(value);
        }
    }
}
