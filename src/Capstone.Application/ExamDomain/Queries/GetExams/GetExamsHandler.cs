namespace Capstone.Application.ExamDomain.Queries.GetExams
{
    public class GetExamsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamsQuery, GetExamsResult>
    {
        public async Task<GetExamsResult> Handle(GetExamsQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);

            var examsQuery = dbContext.Exams.AsNoTracking()
                                      .Join(dbContext.ExamTemplates,
                                            e => e.ExamTemplateId,
                                            et => et.Id,
                                            (e, et) => new { e, et })
                                      .Join(dbContext.TeacherSubjects,
                                            t => t.et.SubjectId,
                                            s => s.Id,
                                            (t, s) => new { t.e, t.et, s }
                                      );

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await examsQuery.CountAsync(cancellationToken);

            var exams = await examsQuery
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(t => new GetExamsDto(
                                    t.e.Id.Value,
                                    t.e.Title.Value,
                                    t.et.Id.Value,
                                    t.et.Title.Value,
                                    t.s.Id.Value,
                                    t.s.SubjectName.Value,
                                    t.e.Duration.Value,
                                    t.e.ExamVersions.Count
                                    ))
                                .ToListAsync(cancellationToken);

            return new GetExamsResult(new PaginationResult<GetExamsDto>(
                pageIndex,
                pageSize,
                totalCount,
                exams
                ));
        }
    }
}
