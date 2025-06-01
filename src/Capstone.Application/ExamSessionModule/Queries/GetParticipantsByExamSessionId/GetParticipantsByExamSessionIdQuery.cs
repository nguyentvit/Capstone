namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsByExamSessionId
{
    public record GetParticipantsByExamSessionIdQuery(Guid UserId, Guid ExamSessionId, bool IsFree, PaginationRequest PaginationRequest) : IQuery<GetParticipantsByExamSessionIdResult>;
    public record GetParticipantsByExamSessionIdResult(PaginationResult<GetParticipantsByExamSessionIdDto> Participants);
    public record GetParticipantsByExamSessionIdDto(string? UserName, DateTime? StartTime, DateTime? EndTime, TimeSpan? TotalTime, double? Result);
}
