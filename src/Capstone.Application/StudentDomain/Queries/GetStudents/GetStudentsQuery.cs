namespace Capstone.Application.StudentDomain.Queries.GetStudents
{
    public record GetStudentsQuery(string KeySearchStudentId, PaginationRequest PaginationRequest) : IQuery<GetStudentsResult>;
    public record GetStudentsResult(PaginationResult<GetStudentsDto> Students);
    public record GetStudentsDto(Guid Id, string StudentId, string StudentName);
}
