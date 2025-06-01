using Capstone.Domain.ChapterDomain.ValueObjects;

namespace Capstone.Application.ChapterDomain.Queries.GetChapterById
{
    public class GetChapterByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetChapterByIdQuery, GetChapterByIdResult>
    {
        public async Task<GetChapterByIdResult> Handle(GetChapterByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var chapterId = ChapterId.Of(query.ChapterId);

            var chapter = await dbContext.Chapters
                                         .AsNoTracking()
                                         .Where(c => c.Id == chapterId)
                                         .Select(c => new {c.SubjectId, c.Title})
                                         .FirstOrDefaultAsync(cancellationToken);

            if (chapter == null)
                throw new ChapterNotFoundException(chapterId.Value);

            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == chapter.SubjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(chapter.SubjectId.Value);

            SubjectExtention.CheckRole(subject, userId, query.Role);

            return new GetChapterByIdResult(chapter.Title.Value);
        }
    }
}
