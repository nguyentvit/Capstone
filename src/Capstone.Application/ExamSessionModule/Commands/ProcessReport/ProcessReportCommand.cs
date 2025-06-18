namespace Capstone.Application.ExamSessionModule.Commands.ProcessReport
{
    public record ProcessReportCommand(Guid ParticipantId, Guid QuestionId, double Score) : ICommand<ProcessReportResult>;
    public record ProcessReportResult(bool IsSuccess);
}
