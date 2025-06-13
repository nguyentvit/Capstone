using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.Application.FreeParticipant.Commands.AddAction
{
    public record AddActionCommand(Guid ParticipantId, ActionType ActionType) : ICommand<AddActionResult>;
    public record AddActionResult(bool IsSuccess);
}
