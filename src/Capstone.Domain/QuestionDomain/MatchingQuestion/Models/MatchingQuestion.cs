using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Entities;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.QuestionDomain.MatchingQuestion.Models
{
    public class MatchingQuestion : Question
    {
        private readonly List<MatchingPair> _matchingPairs = new();
        public IReadOnlyList<MatchingPair> MatchingPairs => _matchingPairs.AsReadOnly();
        private MatchingQuestion() { }
        private MatchingQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, List<MatchingPair> matchingPairs)
            : base(title, content, difficulty, QuestionType.MatchingPairQuestion, userId, chapterId)
        {
            _matchingPairs = matchingPairs;
        }
        public static MatchingQuestion Of(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, UserId userId, ChapterId? chapterId, List<MatchingPair> matchingPairs)
        {
            return new MatchingQuestion(title, content, difficulty, userId, chapterId, matchingPairs);
        }
        public MatchingQuestion UpdateQuestion(QuestionTitle title, QuestionContent content, QuestionDifficulty difficulty, List<MatchingPair> matchingPairs)
        {
            var updatedQuestion = new MatchingQuestion(
                title,
                content,
                difficulty,
                UserId,
                ChapterId,
                matchingPairs
                );

            updatedQuestion.SetVersioning(RootId, Id);

            SetIsLastVersionIsFalse();

            return updatedQuestion;
        }
    }
}
