namespace Capstone.Application.AdminDomain.Queries.GetStudents
{
    public record GetStudentsQuery(PaginationRequest PaginationRequest) : IQuery<GetStudentsResult>;
    public record GetStudentsResult(PaginationResult<GetStudentsResultDto> Students);
    public record GetStudentsResultDto(Guid Id, string StudentId, string UserName, string? Email, string? PhoneNumber, string? Avatar, bool IsActive);
}
