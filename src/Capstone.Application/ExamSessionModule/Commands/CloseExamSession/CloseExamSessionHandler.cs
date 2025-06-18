using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.CloseExamSession
{
    public class CloseExamSessionHandler(IApplicationDbContext dbContext) : ICommandHandler<CloseExamSessionCommand, CloseExamSessionResult>
    {
        public async Task<CloseExamSessionResult> Handle(CloseExamSessionCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.UserId);
            var examSessionId = ExamSessionId.Of(command.ExamSessionId);

            var examSession = await dbContext.ExamSessions
                                             .Where(es => es.Id == examSessionId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (examSession.UserId.Value != userId.Value)
                throw new AccessNotAllowException();

            if (examSession.IsDone.Value)
                throw new BadRequestException("Kì thi này đã kết thúc");

            if (!examSession.IsClosePoint.Value)
                throw new BadRequestException("Bạn phải chốt điểm trước khi kết thúc kì thi");

            examSession.CloseExamSession();

            await dbContext.SaveChangesAsync(cancellationToken);

            return new CloseExamSessionResult(true);
        }
    }
}
