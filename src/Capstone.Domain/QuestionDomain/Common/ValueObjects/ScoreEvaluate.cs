namespace Capstone.Domain.QuestionDomain.Common.ValueObjects
{
    public record ScoreEvaluate
    {
        public int Value { get; }
        private ScoreEvaluate(int value) => Value = value;
        public static ScoreEvaluate Of(int value)
        {
            return new ScoreEvaluate(value);
        }
    }
}
