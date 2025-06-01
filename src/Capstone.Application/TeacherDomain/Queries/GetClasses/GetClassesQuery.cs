namespace Capstone.Application.TeacherDomain.Queries.GetClasses
{
    public record GetClassesQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetClassesResult>;
    public record GetClassesResult(PaginationResult<GetClassesDto> Classes);
    public record GetClassesDto(Guid Id, string ClassName);
}
