namespace Capstone.Application.Admin.Queries.GetUsers;
public class GetUsersHandler : IQueryHandler<GetUsersQuery, GetUsersResult>
{
    public Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
