namespace Capstone.Application.AdminDomain.Queries.GetStudentsAdmin
{
    public record GetStudentsAdminQuery(PaginationRequest PaginationRequest) : IQuery<GetStudentsAdminResult>;
    public record GetStudentsAdminResult(PaginationResult<GetStudentsAdminDto> Students);
    public record GetStudentsAdminDto(Guid Id, string StudentId, string UserName, string? Email, string? PhoneNumber, string? Avartar, bool IsActive);
}
