using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.MarkLastVersion
{
    public class MarkLastVersionHandler(IApplicationDbContext dbContext) : ICommandHandler<MarkLastVersionCommand, MarkLastVersionResult>
    {
        public async Task<MarkLastVersionResult> Handle(MarkLastVersionCommand command, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(command.QuestionId);
            var question = await dbContext.Questions
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(command.QuestionId);

            if (question.UserId.Value != command.UserId)
                throw new AccessNotAllowException();

            if (question.IsLastVersion.Value == true)
                return new MarkLastVersionResult(true);

            var questionIsLastVerion = await dbContext.Questions
                                                      .Where(q => q.RootId == question.RootId && q.IsLastVersion == IsLastVersion.Of(true))
                                                      .FirstOrDefaultAsync(cancellationToken);

            if (questionIsLastVerion == null)
                throw new BadRequestException("Không tìm thấy phiên bản sử dụng hiện tại");

            if (question.ChapterId != null)
            {
                var chapter = await dbContext.Chapters
                                             .Where(c => c.Id == question.ChapterId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (chapter == null)
                    throw new ChapterNotFoundException(question.ChapterId.Value);

                chapter.UpdateQuestionVersioning(questionIsLastVerion.Id, questionId);
            }

            question.SetIsLastVersionIsTrue();
            questionIsLastVerion.SetIsLastVersionIsFalsePublic();

            await dbContext.SaveChangesAsync(cancellationToken);

            return new MarkLastVersionResult(true);
        }
    }
}
