namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantActionByExamSessionId
{
    public record GetParticipantActionByExamSessionIdQuery(Guid UserId, Guid ExamSessionId, GetParticipantActionByExamSessionIdCondition Condition, PaginationRequest PaginationRequest) : IQuery<GetParticipantActionByExamSessionIdResult>;
    public record GetParticipantActionByExamSessionIdResult(PaginationResult<GetParticipantActionByExamSessionIdDto> ParticipantActions);
    public record GetParticipantActionByExamSessionIdDto(string UserName, GetParticipantActionByExamSessionIdCondition Status, bool IsFree);
    public enum GetParticipantActionByExamSessionIdCondition
    {
        All = 0,
        NotStarted = 1,
        Done = 2,
        Examing = 3
    }
}
