namespace Capstone.Application.RescueTeam.Queries.GetMembersByRescueId;
public record GetMembersByRescueIdQuery(Guid RescueId, PaginationRequest PaginationRequest) : IQuery<GetMembersByRescueIdResult>;
public record GetMembersByRescueIdResult(PaginationResult<GetMembersByRescueIdDto> Members);