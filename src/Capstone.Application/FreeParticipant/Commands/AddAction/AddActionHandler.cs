using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.FreeParticipant.Commands.AddAction
{
    public class AddActionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddActionCommand, AddActionResult>
    {
        public async Task<AddActionResult> Handle(AddActionCommand command, CancellationToken cancellationToken)
        {
            var examSession = await dbContext.ExamSessions
                                             .SelectMany(es => es.Participants, (es, p) => new
                                             {
                                                 es,
                                                 p
                                             })
                                             .Where(t => t.p.Id == ParticipantId.Of(command.ParticipantId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            if (examSession.p.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể tham gia");

            if (examSession.p.ExamVersionId == null)
                throw new BadRequestException("Bài thi chưa được bắt đầu");

            examSession.p.AddAction(command.ActionType);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddActionResult(true);
        }
    }
}
