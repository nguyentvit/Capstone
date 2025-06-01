using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Entities;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models
{
    public class MultiChoiceQuestion : Question
    {
        private readonly List<ChoiceMulti> _choices = new();
        public IReadOnlyList<ChoiceMulti> Choices => _choices.AsReadOnly();
        private MultiChoiceQuestion() { }
        private MultiChoiceQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId,ChapterId? chapterId, List<ChoiceMulti> choices)
            : base(title, content, difficulty, QuestionType.MultiChoiceQuestion, userId, chapterId)
        {
            _choices = choices;
        }
        public static MultiChoiceQuestion Of(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, List<ChoiceMulti> choices)
        {
            return new MultiChoiceQuestion(title, content, difficulty, userId, chapterId, choices);
        }
        public MultiChoiceQuestion UpdateQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, List<ChoiceMulti> choices)
        {
            var updatedQuestion = new MultiChoiceQuestion(
                title,
                content,
                difficulty,
                UserId,
                ChapterId,
                choices
                );

            updatedQuestion.SetVersioning(RootId, Id);

            SetIsLastVersionIsFalse();

            return updatedQuestion;
        }
    }
}
