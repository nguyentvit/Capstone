using Capstone.Domain.ExamTemplateModule.Entities;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Domain.ExamTemplateModule.Models
{
    public class ExamTemplate : Aggregate<ExamTemplateId>
    {
        private readonly List<ExamTemplateSection> _examTemplateSection = new();
        public IReadOnlyList<ExamTemplateSection> ExamTemplateSection => _examTemplateSection.AsReadOnly();
        public SubjectId SubjectId { get; private set; } = default!;
        public ExamTemplateTitle Title { get; private set; } = default!;
        public ExamTemplateDescription Description { get; private set; } = default!;
        public ExamTemplateDuration DurationInMinutes { get; private set; } = default!;
        public IsActive IsActive { get; private set; } = default!;
        public IsDeleted IsDeleted { get; private set; } = default!;
        private ExamTemplate(SubjectId subjectId, ExamTemplateTitle title, ExamTemplateDescription description, ExamTemplateDuration durationInMinutes)
        {
            Id = ExamTemplateId.Of(Guid.NewGuid());
            SubjectId = subjectId;
            Title = title;
            Description = description;
            DurationInMinutes = durationInMinutes;
            IsActive = IsActive.Of(true);
            IsDeleted = IsDeleted.Of(false);
        }
        public static ExamTemplate Of(SubjectId subjectId, ExamTemplateTitle title, ExamTemplateDescription description, ExamTemplateDuration durationInMinutes)
        {
            return new ExamTemplate(subjectId, title, description, durationInMinutes);
        }
        public void AddExamTemplateSection(ExamTemplateSection examTemplateSection)
        {
            if (_examTemplateSection.Any(s => s.ChapterId == examTemplateSection.ChapterId))
            {
                throw new DomainException($"Đã tồn tại chương với Id '{examTemplateSection.ChapterId.Value}' trong khung đề thi");
            }

            _examTemplateSection.Add(examTemplateSection);
        }
    }
}
