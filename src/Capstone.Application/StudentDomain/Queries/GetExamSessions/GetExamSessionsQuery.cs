namespace Capstone.Application.StudentDomain.Queries.GetExamSessions
{
    public record GetExamSessionsQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetExamSessionsResult>;
    public record GetExamSessionsResult(PaginationResult<GetExamSessionsDto> ExamSessions);
    public record GetExamSessionsDto(Guid Id, string Name, DateTime StartTime, DateTime EndTime, int Duration, bool IsDone);
}
