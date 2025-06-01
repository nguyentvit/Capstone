namespace Capstone.Application.AdminDomain.Queries.GetTeacherById
{
    public record GetTeacherByIdQuery(Guid Id) : IQuery<GetTeacherByIdResult>;
    public record GetTeacherByIdResult(Guid Id, string TeacherId, string UserName, string? Email, string? PhoneNumber, string? Avartar, bool IsActive);
}
