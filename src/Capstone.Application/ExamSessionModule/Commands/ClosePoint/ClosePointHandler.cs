using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.ClosePoint
{
    public class ClosePointHandler(IApplicationDbContext dbContext) : ICommandHandler<ClosePointCommand, CLosePointResult>
    {
        public async Task<CLosePointResult> Handle(ClosePointCommand command, CancellationToken cancellationToken)
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

            if (examSession.IsClosePoint.Value)
                throw new BadRequestException("Kì thi này đã chốt điểm");

            foreach (var participant in examSession.Participants)
            {
                foreach (var answer in participant.Answers)
                {
                    if (answer.Score == null)
                    {
                        answer.Grade(0);
                    }
                }
            }

            examSession.ClosePoint();

            await dbContext.SaveChangesAsync(cancellationToken);

            return new CLosePointResult(true);
        }
    }
}
