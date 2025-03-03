namespace Capstone.Application.RescueTeam.Queries.GetRescueById;
public record GetRescueByIdQuery(Guid Id) : IQuery<GetRescueByIdResult>;
public record GetRescueByIdResult(GetRescueByIdDto Rescue);