namespace Capstone.Application.StudentDomain.Commands.AddReport
{
    public record AddReportCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId) : ICommand<AddReportResult>;
    public record AddReportResult(bool IsSuccess);
}
