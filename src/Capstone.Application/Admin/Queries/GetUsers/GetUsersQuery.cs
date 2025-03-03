namespace Capstone.Application.Admin.Queries.GetUsers;

public record GetUsersQuery(PaginationRequest PaginationRequest) : IQuery<GetUsersResult>;
public record GetUsersResult(PaginationResult<GetUsersDto> PaginationResult);