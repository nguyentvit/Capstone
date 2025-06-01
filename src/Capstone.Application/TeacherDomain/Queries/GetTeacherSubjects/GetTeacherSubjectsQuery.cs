namespace Capstone.Application.TeacherDomain.Queries.GetTeacherSubjects
{
    public record GetTeacherSubjectsQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetTeacherSubjectsResult>;
    public record GetTeacherSubjectsResult(PaginationResult<GetTeacherSubjectsDto> Subjects);
    public record GetTeacherSubjectsDto(Guid Id, string SubjectName);
}
