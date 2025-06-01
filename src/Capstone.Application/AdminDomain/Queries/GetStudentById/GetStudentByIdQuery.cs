namespace Capstone.Application.AdminDomain.Queries.GetStudentById
{
    public record GetStudentByIdQuery(Guid Id) : IQuery<GetStudentByIdResult>;
    public record GetStudentByIdResult(Guid Id, string StudentId, string UserName, string? Email, string? PhoneNumber, string? Avatar, bool IsActive);
}
