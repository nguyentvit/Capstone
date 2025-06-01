namespace Capstone.Application.ChapterDomain.Queries.GetChapterById
{
    public record GetChapterByIdQuery(Guid UserId, string Role, Guid ChapterId) : IQuery<GetChapterByIdResult>;
    public record GetChapterByIdResult(string Title);
}
