namespace Capstone.Application.StudentDomain.Queries.GetJoinedClasses
{
    public record GetJoinedClassesQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetJoinedClassesResult>;
    public record GetJoinedClassesResult(PaginationResult<GetJoinedClassesDto> Classes);
    public record GetJoinedClassesDto(Guid Id, string ClassName);
}
