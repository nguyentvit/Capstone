using Capstone.Domain.ChapterDomain.ValueObjects;

namespace Capstone.Application.QuestionDomain.Queries.GetQuestionsByChapterId
{
    public class GetQuestionsByChapterIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetQuestionsByChapterIdQuery, GetQuestionsByChapterIdResult>
    {
        public async Task<GetQuestionsByChapterIdResult> Handle(GetQuestionsByChapterIdQuery query, CancellationToken cancellationToken)
        {
            var chapterId = ChapterId.Of(query.ChapterId);
            var chapter = await dbContext.Chapters
                                         .AsNoTracking()
                                         .Where(c => c.Id == chapterId)
                                         .Select(c => new { c.SubjectId, c.QuestionIds })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (chapter == null)
                throw new ChapterNotFoundException(chapterId.Value);

            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == chapter.SubjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(chapter.SubjectId.Value);

            var userId = UserId.Of(query.UserId);

            SubjectExtention.CheckRole(subject, userId, query.Role);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = chapter.QuestionIds.Count;

            var questionIds = chapter.QuestionIds;

            var questions = await dbContext.Questions
                                           .AsNoTracking()
                                           .Where(q => questionIds.Contains(q.Id))
                                           .Select(q => QuestionExtension.ConvertToQuestionDto(q))
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync(cancellationToken);

            return new GetQuestionsByChapterIdResult(new PaginationResult<QuestionBaseDto>(
                pageIndex,
                pageSize,
                totalCount,
                questions
                ));

        }
    }
}
