using Capstone.Domain.ExamSessionModule.Enums;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Domain.ExamSessionModule.Entities
{
    public class ParticipantAnswer : Entity<ParticipantAnswerId>
    {
        public QuestionId QuestionId { get; private set; } = default!;
        public AnswerRaw AnswerRaw { get; private set; } = default!;
        public Score? Score { get; private set; }
        public GradingStatus GradingStatus { get; private set; } = default!;
        private ParticipantAnswer() { }
        private ParticipantAnswer(QuestionId questionId, AnswerRaw answerRaw)
        {
            Id = ParticipantAnswerId.Of(Guid.NewGuid());
            QuestionId = questionId;
            AnswerRaw = answerRaw;
        }
        public static ParticipantAnswer Of(QuestionId questionId, string answer)
        {
            return new ParticipantAnswer(questionId, AnswerRaw.Of(answer));
        }
        public void Grade(double score)
        {
            Score = Score.Of(score);
            GradingStatus = GradingStatus.Graded;
        }
    }
}
