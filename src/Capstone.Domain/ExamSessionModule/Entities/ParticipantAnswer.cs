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
        public Duration Duration { get; private set; } = default!;
        public IsReport IsReport { get; private set; } = default!;
        public IsProcess IsProcess { get; private set; } = default!;
        private ParticipantAnswer() { }
        private ParticipantAnswer(QuestionId questionId, AnswerRaw answerRaw, Duration duration)
        {
            Id = ParticipantAnswerId.Of(Guid.NewGuid());
            QuestionId = questionId;
            AnswerRaw = answerRaw;
            Duration = duration;
            IsReport = IsReport.Of(true);
            IsProcess = IsProcess.Of(false);
        }
        public static ParticipantAnswer Of(QuestionId questionId, string answer, TimeSpan duration)
        {
            return new ParticipantAnswer(questionId, AnswerRaw.Of(answer), Duration.Of(duration));
        }
        public void Grade(double score)
        {
            Score = Score.Of(score);
            GradingStatus = GradingStatus.Graded;
        }
        public void AddReport()
        {
            IsReport = IsReport.Of(true);
        }
        public void ProcessReport(double score)
        {
            IsProcess = IsProcess.Of(true);
            Score = Score.Of(score);
        }
    }
}
