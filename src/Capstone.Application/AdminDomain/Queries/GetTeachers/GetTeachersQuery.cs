namespace Capstone.Application.AdminDomain.Queries.GetTeachers
{
    public record GetTeachersQuery(PaginationRequest PaginationRequest) : IQuery<GetTeachersResult>;
    public record GetTeachersResult(PaginationResult<GetTeachersResultDto> Teachers);
    public record GetTeachersResultDto(Guid Id, string TeacherId, string UserName, string? Email, string? PhoneNumber, string? Avartar, bool IsActive);
}
