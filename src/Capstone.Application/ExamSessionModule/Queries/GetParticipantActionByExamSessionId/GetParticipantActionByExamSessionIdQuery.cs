namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantActionByExamSessionId
{
    public record GetParticipantActionByExamSessionIdQuery(Guid UserId, Guid ExamSessionId, GetParticipantActionByExamSessionIdCondition Condition) : IQuery<GetParticipantActionByExamSessionIdResult>;
    public record GetParticipantActionByExamSessionIdResult(List<GetParticipantActionByExamSessionIdDto> ParticipantActions);
    public record GetParticipantActionByExamSessionIdDto(string? StudentId, string UserName, bool IsFree);
    public enum GetParticipantActionByExamSessionIdCondition
    {
        All = 0,
        NotStarted = 1,
        Done = 2,
        Examing = 3
    }
}
