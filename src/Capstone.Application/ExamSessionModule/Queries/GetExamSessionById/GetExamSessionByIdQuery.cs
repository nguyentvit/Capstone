namespace Capstone.Application.ExamSessionModule.Queries.GetExamSessionById
{
    public record GetExamSessionByIdQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetExamSessionByIdResult>;
    public record GetExamSessionByIdResult(string Name, DateTime StartDate, DateTime EndDate, int Duration, bool IsCodeBased, string? Code, Guid ExamId, string ExamName);

}
