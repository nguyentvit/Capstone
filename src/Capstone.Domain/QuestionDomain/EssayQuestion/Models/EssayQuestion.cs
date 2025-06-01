using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.EssayQuestion.Models
{
    public class EssayQuestion : Question
    {
        private EssayQuestion() { }
        public EssayQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId)
            : base(title, content, difficulty, QuestionType.EssayQuestion, userId, chapterId) { }
        public static EssayQuestion Of(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId)
        {
            return new EssayQuestion(title, content, difficulty, userId, chapterId);
        }
        public EssayQuestion UpdateQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty)
        {
            var updatedQuestion = new EssayQuestion(
                title,
                content,
                difficulty,
                UserId,
                ChapterId
                );

            updatedQuestion.SetVersioning(RootId, Id);

            SetIsLastVersionIsFalse();

            return updatedQuestion;
        }
    }
}
