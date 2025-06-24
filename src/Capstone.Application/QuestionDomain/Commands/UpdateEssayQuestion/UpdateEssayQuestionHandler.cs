using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.UpdateEssayQuestion
{
    public class UpdateEssayQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateEssayQuestionCommand, UpdateEssayQuestionResult>
    {
        public async Task<UpdateEssayQuestionResult> Handle(UpdateEssayQuestionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.EssayQuestions
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(questionId.Value);

            var userId = UserId.Of(command.UserId);
            if (question.UserId != userId)
                throw new BadRequestException("Bạn không có quyền cập nhật câu hỏi này");

            var updatedQuestion = question.UpdateQuestion(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty
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

            dbContext.EssayQuestions.Add(updatedQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateEssayQuestionResult(updatedQuestion.Id.Value);
        }
    }
}
