using Capstone.Domain.RescueTeam.ValueObjects;

namespace Capstone.Application.RescueTeam.Queries.GetRescueById;
public class GetRescueByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetRescueByIdQuery, GetRescueByIdResult>
{
    public async Task<GetRescueByIdResult> Handle(GetRescueByIdQuery query, CancellationToken cancellationToken)
    {
        var rescueId = RescueId.Of(query.Id);
        var rescue = await (
            from r in dbContext.Rescues
            where r.Id == rescueId
            select new GetRescueByIdDto(
                r.Id.Value,
                r.RescueName.Value,
                r.Phone.Value
            )).FirstOrDefaultAsync(cancellationToken);

        if (rescue == null)
            throw new RescueNotFoundException(query.Id);

        return new GetRescueByIdResult(rescue);
    }
}
