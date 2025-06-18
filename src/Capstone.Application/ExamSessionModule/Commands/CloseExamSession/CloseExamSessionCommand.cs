namespace Capstone.Application.ExamSessionModule.Commands.CloseExamSession
{
    public record CloseExamSessionCommand(Guid UserId, Guid ExamSessionId) : ICommand<CloseExamSessionResult>;
    public record CloseExamSessionResult(bool IsSuccess);
}
