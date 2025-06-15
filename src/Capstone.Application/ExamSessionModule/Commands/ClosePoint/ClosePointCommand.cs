namespace Capstone.Application.ExamSessionModule.Commands.ClosePoint
{
    public record ClosePointCommand(Guid UserId, Guid ExamSessionId) : ICommand<CLosePointResult>;
    public record CLosePointResult(bool IsSuccess);
}
