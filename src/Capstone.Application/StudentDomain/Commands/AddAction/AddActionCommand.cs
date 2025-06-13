using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.Application.StudentDomain.Commands.AddAction
{
    public record AddActionCommand(Guid UserId, Guid ExamSessionId, ActionType ActionType) : ICommand<AddActionResult>;
    public record AddActionResult(bool IsSuccess);
}
