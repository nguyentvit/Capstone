using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsDoingByExamSessionId
{
    public record GetParticipantsDoingByExamSessionIdQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetParticipantsDoingByExamSessionIdResult>;
    public record GetParticipantsDoingByExamSessionIdResult(List<GetParticipantsDoingByExamSessionIdDto> ParticipantDoing);
    public record GetParticipantsDoingByExamSessionIdDto(string UserName, ActionType ActionType, DateTime CreatedAt);
}
