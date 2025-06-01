namespace Capstone.Application.StudentDomain.Commands.SubmitExamSession
{
    public record SubmitExamSessionCommand(Guid UserId, Guid ExamSessionId) : ICommand<SubmitExamSessionResult>;
    public record SubmitExamSessionResult(double Score, double FullScore);
}
