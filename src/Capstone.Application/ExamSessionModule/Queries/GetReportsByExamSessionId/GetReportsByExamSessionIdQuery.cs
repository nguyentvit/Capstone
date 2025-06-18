namespace Capstone.Application.ExamSessionModule.Queries.GetReportsByExamSessionId
{
    public record GetReportsByExamSessionIdQuery(Guid UserId, Guid ExamSessionId, bool? IsProcess) : IQuery<GetReportsByExamSessionIdResult>;
    public record GetReportsByExamSessionIdResult(List<GetReportsByExamSessionIdDto> Reports);
    public record GetReportsByExamSessionIdDto(Guid ParticipantId, string StudentId, string UserName, List<GetReportsByExamSessionIdProcess> Questions);
    public record GetReportsByExamSessionIdProcess(QuestionBaseWithAnswerDto Question, bool IsProcess);
}
