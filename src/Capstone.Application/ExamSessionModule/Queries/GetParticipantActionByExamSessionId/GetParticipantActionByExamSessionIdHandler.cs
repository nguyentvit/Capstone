using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantActionByExamSessionId
{
    public class GetParticipantActionByExamSessionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetParticipantActionByExamSessionIdQuery, GetParticipantActionByExamSessionIdResult>
    {
        public async Task<GetParticipantActionByExamSessionIdResult> Handle(GetParticipantActionByExamSessionIdQuery query, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);
            var examSession = await dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .Select(es => new { es.UserId })
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (examSession.UserId.Value != query.UserId)
                throw new AccessNotAllowException();

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var examSessionQuery = dbContext.ExamSessions
                                            .AsNoTracking()
                                            .Where(es => es.Id == examSessionId)
                                            .SelectMany(es => es.Participants)
                                            .GroupJoin(dbContext.Students,
                                                        p => p.StudentId,
                                                        s => s.StudentId,
                                                        (p, s) => new {p, s})
                                            .SelectMany(
                                                t => t.s.DefaultIfEmpty(),
                                                (t, student) => new
                                                {
                                                    t.p,
                                                    student
                                                });

            var totalCount = await examSessionQuery.CountAsync(cancellationToken);
            var result = await examSessionQuery
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(t => new GetParticipantActionByExamSessionIdDto(t.p.FullName.Value, GetParticipantActionByExamSessionIdCondition.All, true))
                                        .ToListAsync(cancellationToken);

            return new GetParticipantActionByExamSessionIdResult(new PaginationResult<GetParticipantActionByExamSessionIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                result  
                ));
        }
    }
}
