using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Domain.SubjectDomain.Models
{
    public class Subject : Aggregate<SubjectId>
    {
        private readonly List<ChapterId> _chapterIds = new();
        public IReadOnlyList<ChapterId> ChapterIds => _chapterIds.AsReadOnly();
        public SubjectName SubjectName { get; set; } = default!;
        public IsDeleted IsDeleted { get; set; } = default!;
        protected Subject() { }
        protected Subject(SubjectName subjectName)
        {
            Id = SubjectId.Of(Guid.NewGuid());
            SubjectName = subjectName;
            IsDeleted = IsDeleted.Of(false);
        }
        public void AddChapterId(ChapterId id)
        {
            _chapterIds.Add(id);
        }
    }
}
