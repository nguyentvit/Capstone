using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models
{
    public class SingleChoiceQuestion : Question
    {
        private readonly List<ChoiceSingle> _choices = new();
        public IReadOnlyList<ChoiceSingle> Choices => _choices.AsReadOnly();
        public ChoiceSingleId CorrectAnswerId { get; private set; } = default!;
        private SingleChoiceQuestion() { }
        private SingleChoiceQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, List<ChoiceSingle> choices, ChoiceSingleId choiceId)
            : base(title, content, difficulty, QuestionType.SingleChoiceQuestion, userId, chapterId)
        {
            _choices = choices;
            CorrectAnswerId = choiceId;
        }
        public static SingleChoiceQuestion Of(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, List<ChoiceSingle> choices, ChoiceSingleId choiceId)
        {
            return new SingleChoiceQuestion(title, content, difficulty, userId, chapterId, choices, choiceId);
        }
        public SingleChoiceQuestion UpdateQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, List<ChoiceSingle> choices, ChoiceSingleId choiceId)
        {
            var updatedQuestion = new SingleChoiceQuestion(
                title,
                content,
                difficulty,
                UserId,
                ChapterId,
                choices,
                choiceId
                );

            updatedQuestion.SetVersioning(RootId, Id);

            SetIsLastVersionIsFalse();

            return updatedQuestion;
        }
    }
}
