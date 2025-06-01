namespace Capstone.Application.AdminDomain.Queries.GetSystemSubjects
{
    public record GetSystemSubjectsQuery(PaginationRequest PaginationRequest) : IQuery<GetSystemSubjectsResult>;
    public record GetSystemSubjectsResult(PaginationResult<GetSystemSubjectsDto> Subjects);
    public record GetSystemSubjectsDto(Guid Id, string SubjectName);
}
