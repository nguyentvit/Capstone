using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamSessionModule.Enums;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.StudentDomain.ValueObjects;
using Newtonsoft.Json;

namespace Capstone.Domain.ExamSessionModule.Entities
{
    public class Participant : Entity<ParticipantId>
    {
        private readonly List<ParticipantAction> _actions = new();
        private readonly List<ParticipantAnswer> _answers = new();
        public IReadOnlyList<ParticipantAction> Actions => _actions.AsReadOnly();
        public IReadOnlyList<ParticipantAnswer> Answers => _answers.AsReadOnly();
        public StudentId? StudentId { get; private set; }
        public FullName? FullName { get; private set; }
        public Email? Email { get; private set; }
        public IsFree IsFree { get; private set; } = default!;
        public ExamVersionId? ExamVersionId { get; private set; }
        public IsDone IsDone { get; private set; } = default!;
        public Date? StartAt { get; private set; }
        public Date? SubmittedAt { get; private set; }
        public Score? Score { get; private set; }
        private Participant() { }
        private Participant(StudentId? studentId, FullName? fullName, Email? email, IsFree isFree)
        {
            Id = ParticipantId.Of(Guid.NewGuid());
            StudentId = studentId;
            FullName = fullName;
            Email = email;
            IsFree = isFree;
            IsDone = IsDone.Of(false);
        }
        public bool IsNotStarted()
        {
            return StartAt == null;
        }
        public bool IsDoneStatus()
        {
            return SubmittedAt != null;
        }
        public bool IsExaming()
        {
            return StartAt != null && SubmittedAt == null;
        }
        public static Participant OfStudent(StudentId studentId)
        {
            return new Participant(studentId, null, null, IsFree.Of(false));
        }
        public static Participant OfFreeCandidate(FullName fullName, Email email)
        {
            return new Participant(null, fullName, email, IsFree.Of(true));
        }
        public void AddAction(ActionType actionType)
        {
            _actions.Add(ParticipantAction.Of(actionType));
        }
        public void StartExam(ExamVersionId examVersionId)
        {
            if (StartAt != null)
                throw new DomainException("Bài thi đã bắt đầu");

            ExamVersionId = examVersionId;
            StartAt = Date.Of(DateTime.UtcNow);
            AddAction(ActionType.StartedExam);
        }
        public void SubmitAnswer(QuestionId questionId, object rawAnswerObject)
        {
            var dateNow = DateTime.UtcNow;
            var json = JsonConvert.SerializeObject(rawAnswerObject);

            var totalDuration = _answers
                                .Select(a => a.Duration.Value)
                                .Aggregate(TimeSpan.Zero, (acc, next) => acc + next);

            var duration = dateNow - StartAt!.Value - totalDuration;

            var answer = ParticipantAnswer.Of(questionId, json, duration);

            var existing = _answers.FirstOrDefault(a => a.QuestionId == questionId);
            
            if (existing != null)
            {
                _answers.Remove(existing);
            }

            _answers.Add(answer);
        }
        public void SubmitExam(Dictionary<QuestionId, double?> Scores)
        {
            if (IsDone.Value)
                throw new DomainException("Bài thi đã hoàn thành");

            SubmittedAt = Date.Of(DateTime.UtcNow);
            IsDone = IsDone.Of(true);

            foreach (var key in Scores.Keys)
            {
                var answer = _answers.FirstOrDefault(a => a.QuestionId == key);
                if (answer != null)
                {
                    answer.Grade(Scores[key]);
                }
            }

            AddAction(ActionType.SubmittedExam);
        }
        public void Grade(QuestionId questionId, double score)
        {
            var answer = _answers.FirstOrDefault(a => a.QuestionId == questionId);
            if (answer != null)
            {
                answer.Grade(score);
            }
        }
        public void ProcessReport(QuestionId questionId, double score)
        {
            var answer = _answers.FirstOrDefault(a => a.QuestionId == questionId);
            if (answer != null)
            {
                if (!answer.IsReport.Value)
                    throw new DomainException("Câu hỏi này không được phúc khảo");

                answer.ProcessReport(score);
            }
        }
    }
}
