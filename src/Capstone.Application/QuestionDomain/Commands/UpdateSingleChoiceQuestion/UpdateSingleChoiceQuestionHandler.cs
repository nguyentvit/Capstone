using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.UpdateSingleChoiceQuestion
{
    public class UpdateSingleChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateSingleChoiceQuestionCommand, UpdateSingleChoiceQuestionResult>
    {
        public async Task<UpdateSingleChoiceQuestionResult> Handle(UpdateSingleChoiceQuestionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.SingleChoiceQuestions
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(questionId.Value);

            var userId = UserId.Of(command.UserId);
            if (question.UserId != userId)
                throw new BadRequestException("Bạn không có quyền cập nhật câu hỏi này");

            var choiceEntities = command.Choices.Select(c => ChoiceSingle.Of(ChoiceSingleContent.Of(c))).ToList();
            var correctAnswerId = choiceEntities[command.CorrectAnswerIndex].Id;

            var updatedQuestion = question.UpdateQuestion(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                choiceEntities,
                correctAnswerId
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

            dbContext.SingleChoiceQuestions.Add(updatedQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateSingleChoiceQuestionResult(updatedQuestion.Id.Value);
        }
    }
}
