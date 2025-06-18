using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.ProcessReport
{
    public class ProcessReportHandler(IApplicationDbContext dbContext) : ICommandHandler<ProcessReportCommand, ProcessReportResult>
    {
        public async Task<ProcessReportResult> Handle(ProcessReportCommand command, CancellationToken cancellationToken)
        {
            var examSession = await dbContext.ExamSessions
                                             .Where(es => es.Participants.Any(p => p.Id == ParticipantId.Of(command.ParticipantId)))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new NotFoundException("Không tìm thấy");

            var participant = examSession.Participants.FirstOrDefault(p => p.Id == ParticipantId.Of(command.ParticipantId));

            if (participant == null)
                throw new NotFoundException("Không tìm thấy");

            var questionId = QuestionId.Of(command.QuestionId);

            participant.ProcessReport(questionId, command.Score);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProcessReportResult(true);
        }
    }
}
