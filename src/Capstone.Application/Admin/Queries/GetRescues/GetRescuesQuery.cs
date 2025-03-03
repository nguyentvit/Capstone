namespace Capstone.Application.Admin.Queries.GetRescues;

public record GetRescuesQuery(PaginationRequest PaginationRequest) : IQuery<GetRescuesResult>;
public record GetRescuesResult(PaginationResult<GetRescuesDto> Rescues);