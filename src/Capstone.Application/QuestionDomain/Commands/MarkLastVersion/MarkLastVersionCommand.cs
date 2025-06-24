namespace Capstone.Application.QuestionDomain.Commands.MarkLastVersion
{
    public record MarkLastVersionCommand(Guid UserId, Guid QuestionId) : ICommand<MarkLastVersionResult>;
    public record MarkLastVersionResult(bool IsSuccess);
}
