using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.UpdateMultiChoiceQuestion
{
    public class UpdateMultiChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateMultiChoiceQuestionCommand, UpdateMultiChoiceQuestionResult>
    {
        public async Task<UpdateMultiChoiceQuestionResult> Handle(UpdateMultiChoiceQuestionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.MultiChoiceQuestions
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(questionId.Value);

            var userId = UserId.Of(command.UserId);
            if (question.UserId != userId)
                throw new BadRequestException("Bạn không có quyền cập nhật câu hỏi này");

            var choices = command.Choices.Select(pair =>
            ChoiceMulti.Of(
                ChoiceMultiContent.Of(pair.Key),
                IsCorrect.Of(pair.Value)
            )).ToList();

            var updatedQuestion = question.UpdateQuestion(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                choices
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

            dbContext.MultiChoiceQuestions.Add(updatedQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateMultiChoiceQuestionResult(updatedQuestion.Id.Value);
        }
    }
}
