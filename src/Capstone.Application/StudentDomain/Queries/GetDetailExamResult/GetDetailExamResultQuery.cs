namespace Capstone.Application.StudentDomain.Queries.GetDetailExamResult
{
    public record GetDetailExamResultQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetDetailExamResultResult>;
    public record GetDetailExamResultResult(List<GetDetailExamResult> Questions);
    public record GetDetailExamResult(QuestionBaseWithAnswerDto Question, bool? IsReport, bool? IsProcess);
}
