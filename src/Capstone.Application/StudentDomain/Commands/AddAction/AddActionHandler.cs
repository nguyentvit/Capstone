using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.StudentDomain.Commands.AddAction
{
    public class AddActionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddActionCommand, AddActionResult>
    {
        public async Task<AddActionResult> Handle(AddActionCommand command, CancellationToken cancellationToken)
        {
            var studentId = await dbContext.Students
                                           .AsNoTracking()
                                           .Where(s => s.Id == UserId.Of(command.UserId))
                                           .Select(s => s.StudentId)
                                           .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(command.UserId);

            var examSession = await dbContext.ExamSessions
                                             .SelectMany(e => e.Participants, (es, participants) => new
                                             {
                                                 ExamSession = es,
                                                 Participants = participants
                                             })
                                             .Where(t => t.Participants.StudentId != null && t.Participants.StudentId == studentId && t.ExamSession.Id == ExamSessionId.Of(command.ExamSessionId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            if (examSession.Participants.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể tham gia");

            var dateNow = DateTime.UtcNow;
            if (examSession.ExamSession.StartTime.Value > dateNow || examSession.ExamSession.EndTime.Value < dateNow)
                throw new BadRequestException("Không nằm trong thời gian thi");

            if (examSession.Participants.ExamVersionId == null)
                throw new BadRequestException("Bài thi chưa được bắt đầu");

            examSession.Participants.AddAction(command.ActionType);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddActionResult(true);
        }
    }
}
