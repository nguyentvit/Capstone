namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsResult
{
    public record GetParticipantsResultQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetParticipantsResultResult>;
    public record GetParticipantsResultResult(List<GetParticipantsResultDto> Result);
    public record GetParticipantsResultDto(string? StudentId, string UserName, double Score, double TotalScore, bool IsFree);
}
