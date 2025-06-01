namespace Capstone.Application.UserAccess.Queries.GetUserInfo
{
    public class GetUserInfoHandler(IApplicationDbContext dbContext) : IQueryHandler<GetUserInfoQuery, GetUserInfoResult>
    {
        public async Task<GetUserInfoResult> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var user = await dbContext.AppUsers.FindAsync([userId], cancellationToken);
            if (user == null)
                throw new UserNotFoundException(userId.Value);

            return new GetUserInfoResult(
                Id: user.Id.Value,
                UserName: user.UserName.Value,
                Avatar: user.Avatar?.Url
                );
        }
    }
}
