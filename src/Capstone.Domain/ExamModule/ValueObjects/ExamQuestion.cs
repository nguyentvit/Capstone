using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Domain.ExamModule.ValueObjects
{
    public record ExamQuestion
    {
        public QuestionId QuestionId { get; }
        public int Order { get; }
        public double PointPerCorrect { get; }
        public double PointPerInCorrect { get; }
        private ExamQuestion(QuestionId questionId, int order, double pointPerCorrect, double pointPerInCorrect)
        {
            QuestionId = questionId;
            Order = order;
            PointPerCorrect = pointPerCorrect;
            PointPerInCorrect = pointPerInCorrect;
        }
        public static ExamQuestion Of(QuestionId questionId, int order, double pointPerCorrect, double pointPerInCorrect)
        {
            return new ExamQuestion(questionId, order, pointPerCorrect, pointPerInCorrect);
        }
    }
}
