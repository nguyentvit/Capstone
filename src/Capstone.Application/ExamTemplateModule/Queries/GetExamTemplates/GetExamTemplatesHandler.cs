namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplates
{
    public class GetExamTemplatesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamTemplatesQuery, GetExamTemplatesResult>
    {
        public async Task<GetExamTemplatesResult> Handle(GetExamTemplatesQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);

            var examTemplatesQuery = dbContext.TeacherSubjects.AsNoTracking()
                                         .Join(dbContext.ExamTemplates,
                                                s => s.Id,
                                                et => et.SubjectId,
                                                (s, et) => new { s, et })
                                         .Where(t => t.s.OwnerId == userId);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await examTemplatesQuery.CountAsync(cancellationToken);
            var examTemplates = await examTemplatesQuery
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(t => new GetExamTemplatesDto(
                                            t.et.Id.Value,
                                            t.et.Title.Value,
                                            t.et.Description.Value,
                                            t.et.DurationInMinutes.Value,
                                            t.s.Id.Value,
                                            t.s.SubjectName.Value
                                        ))
                                        .ToListAsync(cancellationToken);

            return new GetExamTemplatesResult(new PaginationResult<GetExamTemplatesDto>(
                pageIndex,
                pageSize,
                totalCount,
                examTemplates
                ));
            
        }
    }
}
