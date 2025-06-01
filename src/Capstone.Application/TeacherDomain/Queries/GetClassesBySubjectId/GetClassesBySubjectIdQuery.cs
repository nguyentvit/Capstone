namespace Capstone.Application.TeacherDomain.Queries.GetClassesBySubjectId
{
    public record GetClassesBySubjectIdQuery(Guid UserId, Guid SubjectId, PaginationRequest PaginationRequest) : IQuery<GetClassesBySubjectIdResult>;
    public record GetClassesBySubjectIdResult(PaginationResult<GetClassesBySubjectIdDto> Classes);
    public record GetClassesBySubjectIdDto(Guid Id, string ClassName);
}
