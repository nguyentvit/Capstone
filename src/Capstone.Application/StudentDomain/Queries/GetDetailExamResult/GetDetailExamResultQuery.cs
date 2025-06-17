namespace Capstone.Application.StudentDomain.Queries.GetDetailExamResult
{
    public record GetDetailExamResultQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetDetailExamResultResult>;
    public record GetDetailExamResultResult();
    public record GetDetailExamResultDto();
}
