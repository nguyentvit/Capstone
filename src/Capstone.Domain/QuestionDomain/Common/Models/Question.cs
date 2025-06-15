using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.Common.Models
{
    public abstract class Question : Aggregate<QuestionId>
    {
        public QuestionTitle Title { get; private set; } = default!;
        public QuestionContent Content { get; private set; } = default!;
        public QuestionDifficulty Difficulty { get; private set; } = default!;
        public QuestionType Type { get; } = default!;
        public ChapterId? ChapterId { get; private set; }
        public UserId UserId { get; private set; } = default!;
        public IsDeleted IsDeleted { get; private set; } = default!;
        public IsActive IsActive { get; private set; } = default!;
        public IsPersonal IsPersonal { get; private set; } = default!;
        public QuestionId? BeforeQuestionId { get; private set; }
        public QuestionId RootId { get; private set; } = default!;
        public IsLastVersion IsLastVersion { get; private set; } = default!;
        public ScoreEvaluate Score { get; private set; }
        protected Question() { }
        protected Question(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, QuestionType type, UserId userId, ChapterId? chapterId)
        {
            Id = QuestionId.Of(Guid.NewGuid());
            Title = title;
            Content = content;
            Difficulty = difficulty;
            Type = type;
            ChapterId = chapterId;
            IsDeleted = IsDeleted.Of(false);
            IsActive = IsActive.Of(true);
            IsPersonal = IsPersonal.Of(chapterId == null);
            UserId = userId;
            RootId = Id;
            BeforeQuestionId = null;
            IsLastVersion = IsLastVersion.Of(true);
            Score = ScoreEvaluate.Of(5);
        }
        protected void SetVersioning(QuestionId rootId, QuestionId beforeQuestionId)
        {
            RootId = rootId;
            BeforeQuestionId = beforeQuestionId;
        }
        protected void SetIsLastVersionIsFalse()
        {
            if (IsLastVersion.Value == false)
            {
                throw new DomainException("Không thể chỉnh sửa phiên bản cũ của câu hỏi");
            }

            IsLastVersion = IsLastVersion.Of(false);
        }
    }
}
