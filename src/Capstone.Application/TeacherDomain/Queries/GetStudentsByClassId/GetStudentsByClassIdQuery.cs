namespace Capstone.Application.TeacherDomain.Queries.GetStudentsByClassId
{
    public record GetStudentsByClassIdQuery(Guid UserId, Guid ClassId, PaginationRequest PaginationRequest) : IQuery<GetStudentsByClassIdResult>;
    public record GetStudentsByClassIdResult(PaginationResult<GetStudentsByClassIdDto> Students);
    public record GetStudentsByClassIdDto(Guid Id, string StudentId, string StudentName);
}
