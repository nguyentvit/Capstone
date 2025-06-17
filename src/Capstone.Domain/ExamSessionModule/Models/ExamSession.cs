using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamSessionModule.Entities;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.ExamSessionModule.Models
{
    public class ExamSession : Aggregate<ExamSessionId>
    {
        private readonly List<Participant> _participants = new();
        public IReadOnlyList<Participant> Participants => _participants.AsReadOnly();
        public ExamSessionName Name { get; private set; } = default!;
        public Date StartTime { get; private set; } = default!;
        public Date EndTime { get; private set;} = default!;
        public ExamSessionDuration Duration { get; private set; } = default!;
        public IsCodeBased IsCodeBased { get; private set; } = default!;
        public ExamSessionCode? Code { get; private set; }
        public ExamId ExamId { get; private set; } = default!;
        public UserId UserId { get; private set; } = default!;
        public ClassId? ClassId { get; private set; }
        public IsDone IsDone { get; private set; } = default!;
        public IsClosePoint IsClosePoint { get; private set; } = default!;
        private ExamSession() { }
        private ExamSession(ExamSessionName name, Date startTime, Date endTime, ExamSessionDuration duration, IsCodeBased isCodeBased, ExamSessionCode? code, ExamId examId, UserId userId, ClassId? classId, List<Participant> participants)
        {
            Id = ExamSessionId.Of(Guid.NewGuid());
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Duration = duration;
            IsCodeBased = isCodeBased;
            Code = code;
            ExamId = examId;
            UserId = userId;
            ClassId = classId;
            _participants = participants;
            IsDone = IsDone.Of(false);
            IsClosePoint = IsClosePoint.Of(false);
        }
        public static ExamSession Of(ExamSessionName name, Date startTime, Date endTime, ExamSessionDuration duration, IsCodeBased isCodeBased, ExamId examId, UserId userId, ClassId? classId, List<Participant> participants)
        {
            if (isCodeBased.Value == true)
            {
                return new ExamSession(name, startTime, endTime, duration, isCodeBased, null, examId, userId, null, participants);
            }
            else
            {
                return new ExamSession(name, startTime, endTime, duration, isCodeBased, ExamSessionCode.Generate(), examId, userId, classId, participants);
            }
        }
        public void AddParticipant(Participant participant)
        {
            _participants.Add(participant);
        }
        public void ClosePoint()
        {
            IsClosePoint = IsClosePoint.Of(true);
        }
    }
}
