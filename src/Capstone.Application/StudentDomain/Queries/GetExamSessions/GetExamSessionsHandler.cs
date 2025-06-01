namespace Capstone.Application.StudentDomain.Queries.GetExamSessions
{
    public class GetExamSessionsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamSessionsQuery, GetExamSessionsResult>
    {
        public async Task<GetExamSessionsResult> Handle(GetExamSessionsQuery query, CancellationToken cancellationToken)
        {
            var studentId = await dbContext.Students
                                         .AsNoTracking()
                                         .Where(s => s.Id == UserId.Of(query.UserId))
                                         .Select(s => s.StudentId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(query.UserId);

            var examSessionsQuery = dbContext.ExamSessions.AsNoTracking()
                                             .SelectMany(e => e.Participants, (es, participants) => new
                                             {
                                                 ExamSession = es,
                                                 Participants = participants
                                             })
                                             .Where(t => t.Participants.StudentId != null && t.Participants.StudentId == studentId);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await examSessionsQuery.CountAsync(cancellationToken);
            var result = await examSessionsQuery
                                            .Select(t => new GetExamSessionsDto(t.ExamSession.Id.Value, t.ExamSession.Name.Value, t.ExamSession.StartTime.Value, t.ExamSession.EndTime.Value, t.ExamSession.Duration.Value, t.Participants.IsDone.Value))
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
