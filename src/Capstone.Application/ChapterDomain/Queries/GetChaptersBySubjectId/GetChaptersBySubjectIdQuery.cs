namespace Capstone.Application.ChapterDomain.Queries.GetChaptersBySubjectId
{
    public record GetChaptersBySubjectIdQuery(Guid UserId, string Role, Guid SubjectId, PaginationRequest PaginationRequest) : IQuery<GetChaptersBySubjectIdResult>;
    public record GetChaptersBySubjectIdResult(PaginationResult<GetChaptersBySubjectIdDto> Chapters);
    public record GetChaptersBySubjectIdDto(Guid Id, string Title);
}
