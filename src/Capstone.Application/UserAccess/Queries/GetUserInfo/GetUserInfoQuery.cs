namespace Capstone.Application.UserAccess.Queries.GetUserInfo
{
    public record GetUserInfoQuery(Guid UserId) : IQuery<GetUserInfoResult>;
    public record GetUserInfoResult(Guid Id, string UserName, string? Avatar);
}
