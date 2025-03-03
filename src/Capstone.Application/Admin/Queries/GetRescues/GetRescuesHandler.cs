
namespace Capstone.Application.Admin.Queries.GetRescues;

public class GetRescuesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetRescuesQuery, GetRescuesResult>
{
    public async Task<GetRescuesResult> Handle(GetRescuesQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        
        var rescuesQuery = (
            from r in dbContext.Rescues
            join u in dbContext.AppUsers on r.ManagerId equals u.Id into userGroup
            from manager in userGroup.DefaultIfEmpty()
            select new GetRescuesDto(
                r.Id.Value,
                r.RescueName.Value,
                r.Phone.Value,
                (r.Avatar == null) ? null : new GetRescuesDtoImage(r.Avatar.Url, r.Avatar.Format.ToString()),
                new GetRescuesDtoAddress(
                    r.Address.District,
                    r.Address.Ward,
                    r.Address.Province,
                    r.Address.Country
                ),
                new GetRescuesDtoCoordinates(
                    r.Coordinates.Latitude,
                    r.Coordinates.Longitude
                ),
                new GetRescuesDtoManager(
                    manager.Id.Value,
                    manager.UserName.Value,
                    (manager.Avatar == null) ? null : new GetRescuesDtoImage(manager.Avatar.Url, manager.Avatar.Format.ToString())
                )
            )
        );
        
        var totalCount = await rescuesQuery.CountAsync(cancellationToken);
        var rescues = await rescuesQuery
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new GetRescuesResult(new PaginationResult<GetRescuesDto>(
            pageIndex,
            pageSize,
            totalCount,
            rescues
        ));
    }
}
