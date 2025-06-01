using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models
{
    public class TrueFalseQuestion : Question
    {
        public IsTrueAnswer IsTrueAnswer { get; private set; } = default!;
        private TrueFalseQuestion() { }
        private TrueFalseQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, IsTrueAnswer isTrueAnswer) 
            : base(title, content, difficulty, QuestionType.TrueFalseQuestion, userId, chapterId)
        {
            IsTrueAnswer = isTrueAnswer;
        }
        public static TrueFalseQuestion Of(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, IsTrueAnswer isTrueAnswer)
        {
            return new TrueFalseQuestion(title, content, difficulty, userId, chapterId, isTrueAnswer);
        }
        public TrueFalseQuestion UpdateQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, IsTrueAnswer isTrueAnswer)
        {
            var updatedQuestion = new TrueFalseQuestion(
                title,
                content,
                difficulty,
                UserId,
                ChapterId,
                isTrueAnswer
                );

            updatedQuestion.SetVersioning(RootId, Id);

            SetIsLastVersionIsFalse();

            return updatedQuestion;
        }
    }
}
