using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Entities;
using Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.UpdateMatchingQuestion
{
    public class UpdateMatchingQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateMatchingQuestionCommand, UpdateMatchingQuestionResult>
    {
        public async Task<UpdateMatchingQuestionResult> Handle(UpdateMatchingQuestionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.MatchingQuestions
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(questionId.Value);

            var userId = UserId.Of(command.UserId);
            if (question.UserId != userId)
                throw new BadRequestException("Bạn không có quyền cập nhật câu hỏi này");

            var pairs = command.MatchingPairs
                               .Select(pair => MatchingPair.Of(MatchingPairContentLeft.Of(pair.Key), MatchingPairContentRight.Of(pair.Value)))
                               .ToList();

            var updatedQuestion = question.UpdateQuestion(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                pairs
                );

            if (question.ChapterId != null)
            {
                var chapter = await dbContext.Chapters
                                             .Where(c => c.Id == question.ChapterId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (chapter == null)
                    throw new ChapterNotFoundException(question.ChapterId.Value);

                chapter.UpdateQuestionVersioning(questionId, updatedQuestion.Id);
            }

            dbContext.MatchingQuestions.Add(updatedQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateMatchingQuestionResult(updatedQuestion.Id.Value);
        }
    }
}
