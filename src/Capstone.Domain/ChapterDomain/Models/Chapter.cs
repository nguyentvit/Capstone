using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Domain.ChapterDomain.Models
{
    public class Chapter : Aggregate<ChapterId>
    {
        private readonly List<QuestionId> _questionIds = new();
        public IReadOnlyList<QuestionId> QuestionIds => _questionIds.AsReadOnly();
        public ChapterTitle Title { get; private set; } = default!;
        public ChapterOrder Order { get; private set; } = default!;
        public SubjectId SubjectId { get; private set; } = default!;
        private Chapter() { }
        private Chapter(ChapterTitle title, ChapterOrder order, SubjectId subjectId)
        {
            Id = ChapterId.Of(Guid.NewGuid());
            Title = title;
            Order = order;
            SubjectId = subjectId;
        }
        public static Chapter Of(ChapterTitle title, ChapterOrder order, SubjectId subjectId)
        {
            return new Chapter(title, order, subjectId);
        }
        public void AddQuestionId(QuestionId questionId)
        {
            _questionIds.Add(questionId);
        }
        public void UpdateQuestionVersioning(QuestionId beforeQuestionId, QuestionId afterQuestionId)
        {
            var index = _questionIds.IndexOf(beforeQuestionId);

            if (index >= 0)
            {
                _questionIds[index] = afterQuestionId;
            }
            else
            {
                _questionIds.Add(afterQuestionId);
            }
        }
    }
}
