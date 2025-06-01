using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.ChapterDomain.Queries.GetChaptersBySubjectId
{
    public class GetChaptersBySubjectIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetChaptersBySubjectIdQuery, GetChaptersBySubjectIdResult>
    {
        public async Task<GetChaptersBySubjectIdResult> Handle(GetChaptersBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var subjectId = SubjectId.Of(query.SubjectId);

            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == subjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            SubjectExtention.CheckRole(subject, userId, query.Role);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var chaptersQuery = dbContext.Chapters
                                         .AsNoTracking()
                                         .Where(c => c.SubjectId == subjectId);

            var totalCount = await chaptersQuery.CountAsync(cancellationToken);

            var chapters = await  chaptersQuery
                                        .OrderBy(c => c.Order)
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(c => new GetChaptersBySubjectIdDto(c.Id.Value, c.Title.Value))
                                        .ToListAsync(cancellationToken);

            return new GetChaptersBySubjectIdResult(new PaginationResult<GetChaptersBySubjectIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                chapters
                ));
        }
    }
}
