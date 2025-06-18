namespace Capstone.Application.TeacherDomain.Queries.GetStudentsAvailableToAddIntoClass
{
    public record GetStudentsAvailableToAddIntoClassQuery(Guid ClassId, string KeySearchStudentId, PaginationRequest PaginationRequest) : IQuery<GetStudentsAvailableToAddIntoClassResult>;
    public record GetStudentsAvailableToAddIntoClassResult(PaginationResult<GetStudentsAvailableToAddIntoClassDto> Students);
    public record GetStudentsAvailableToAddIntoClassDto(Guid Id, string StudentId, string StudentName);
}
