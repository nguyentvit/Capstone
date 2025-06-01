using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Entities;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.AddMatchingQuestion
{
    public class AddMatchingQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddMatchingQuestionCommand, AddMatchingQuestionResult>
    {
        public async Task<AddMatchingQuestionResult> Handle(AddMatchingQuestionCommand command, CancellationToken cancellationToken)
        {
            if (command.ChapterId != null)
            {
                var userId = UserId.Of(command.UserId);
                var chapterId = ChapterId.Of((Guid)command.ChapterId);

                var chapter = await dbContext.Chapters
                                             .Where(s => s.Id == chapterId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (chapter == null)
                    throw new ChapterNotFoundException(chapterId.Value);

                var subject = await dbContext.Subjects
                                             .AsNoTracking()
                                             .Where(s => s.Id == chapter.SubjectId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (subject == null)
                    throw new SubjectNotFoundException(chapter.SubjectId.Value);

                SubjectExtention.CheckRole(subject, userId, command.Role);

                var matchingQuestion = AddMatchingQuestionCommandToMatchingQuestion(command);

                chapter.AddQuestionId(matchingQuestion.Id);
                dbContext.MatchingQuestions.Add(matchingQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddMatchingQuestionResult(matchingQuestion.Id.Value);
            }
            else
            {
                var matchingQuestion = AddMatchingQuestionCommandToMatchingQuestion(command);
                dbContext.MatchingQuestions.Add(matchingQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddMatchingQuestionResult(matchingQuestion.Id.Value);
            }
        }
        private static MatchingQuestion AddMatchingQuestionCommandToMatchingQuestion(AddMatchingQuestionCommand command)
        {
            var pairs = command.MatchingPairs
                               .Select(pair => MatchingPair.Of(MatchingPairContentLeft.Of(pair.Key), MatchingPairContentRight.Of(pair.Value)))
                               .ToList();

            return MatchingQuestion.Of(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                UserId.Of(command.UserId),
                (command.ChapterId != null) ? ChapterId.Of((Guid)command.ChapterId) : null,
                pairs
                );
        }
    }
}
