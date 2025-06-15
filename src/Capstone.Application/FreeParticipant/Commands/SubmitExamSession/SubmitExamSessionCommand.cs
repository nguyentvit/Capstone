namespace Capstone.Application.FreeParticipant.Commands.SubmitExamSession
{
    public record SubmitExamSessionCommand(Guid ParticipantId) : ICommand<SubmitExamSessionResult>;
    public record SubmitExamSessionResult(bool IsSuccess);
}
