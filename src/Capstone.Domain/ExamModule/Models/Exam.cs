using Capstone.Domain.ExamModule.Entities;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.ExamModule.Models
{
    public class Exam : Aggregate<ExamId>
    {
        private readonly List<ExamVersion> _examVersions = new();
        public IReadOnlyList<ExamVersion> ExamVersions => _examVersions.AsReadOnly();
        public ExamTemplateId ExamTemplateId { get; private set; } = default!;
        public ExamDuration Duration { get; private set; } = default!;
        public ExamTitle Title { get; private set; } = default!;
        public UserId UserId { get; private set; } = default!;
        private Exam() { }
        private Exam(ExamTemplateId examTemplateId, ExamDuration duration, ExamTitle title, UserId userId)
        {
            Id = ExamId.Of(Guid.NewGuid());
            ExamTemplateId = examTemplateId;
            Duration = duration;
            Title = title;
            UserId = userId;
        }
        public static Exam Of(ExamTemplateId examTemplateId, ExamDuration duration, ExamTitle title, UserId userId)
        {
            return new Exam(examTemplateId, duration, title, userId);
        }
        public void AddVersion(ExamVersion version)
        {
            _examVersions.Add(version);
        }
    }
}
