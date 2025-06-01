namespace Capstone.Application.ExamSessionModule.Queries.GetExamSessions
{
    public class GetExamSessionsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamSessionsQuery, GetExamSessionsResult>
    {
        public async Task<GetExamSessionsResult> Handle(GetExamSessionsQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var examSessionQuery = dbContext.ExamSessions
                                            .AsNoTracking()
                                            .Join(dbContext.Exams, es => es.ExamId, e => e.Id, 
                                            (es, e) => new {es, e})
                                            .Where(t => t.es.UserId == userId);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await examSessionQuery.CountAsync(cancellationToken);
            
            var result = await examSessionQuery.Select(t => 
                                                        new GetExamSessionsDto(
                                                            t.es.Id.Value,
                                                            t.es.Name.Value,
                                                            t.es.StartTime.Value,
                                                            t.es.EndTime.Value,
                                                            t.es.Duration.Value,
                                                            t.es.IsCodeBased.Value,
                                                            (t.es.Code != null) ? t.es.Code.Value : null,
                                                            t.e.Title.Value,
                                                            t.e.ExamVersions.Count,
                                                            t.es.Participants.Count))
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync(cancellationToken);

            return new GetExamSessionsResult(new PaginationResult<GetExamSessionsDto>(
                pageIndex,
                pageSize,
                totalCount,
                result
                ));
        }
    }
}
