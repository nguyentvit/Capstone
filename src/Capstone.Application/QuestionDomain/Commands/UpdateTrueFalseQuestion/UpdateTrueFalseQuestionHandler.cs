using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.UpdateTrueFalseQuestion
{
    public class UpdateTrueFalseQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateTrueFalseQuestionCommand, UpdateTrueFalseQuestionResult>
    {
        public async Task<UpdateTrueFalseQuestionResult> Handle(UpdateTrueFalseQuestionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.TrueFalseQuestions
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
                (QuestionDifficulty)command.Difficulty,
                IsTrueAnswer.Of(command.IsTrueAnswer)
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

            dbContext.TrueFalseQuestions.Add(updatedQuestion);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateTrueFalseQuestionResult(updatedQuestion.Id.Value);
        }
    }
}
