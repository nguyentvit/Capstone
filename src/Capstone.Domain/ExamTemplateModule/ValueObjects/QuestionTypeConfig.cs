using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record QuestionTypeConfig
    {
        public QuestionType Type { get; private set; }
        public int NumberOfQuestions { get; private set; }
        public double PointPerCorrect { get; private set; }
        public double PointPerInCorrect { get; private set; }
        private QuestionTypeConfig(QuestionType type, int numberOfQuestions, double pointPerCorrect, double pointPerInCorrect)
        {
            Type = type;
            NumberOfQuestions = numberOfQuestions;
            PointPerCorrect = pointPerCorrect;
            PointPerInCorrect = pointPerInCorrect;
        }
        public static QuestionTypeConfig Of(QuestionType type, int numberOfQuestions, double pointPerCorrect, double pointPerInCorrect)
        {
            return new QuestionTypeConfig(type, numberOfQuestions, pointPerCorrect, pointPerInCorrect);
        }

    }
}
