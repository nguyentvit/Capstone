using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsDoingByExamSessionId
{
    public class GetParticipantsDoingByExamSessionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetParticipantsDoingByExamSessionIdQuery, GetParticipantsDoingByExamSessionIdResult>
    {
        public async Task<GetParticipantsDoingByExamSessionIdResult> Handle(GetParticipantsDoingByExamSessionIdQuery query, CancellationToken cancellationToken)
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

            var examSessionQuery = await dbContext.ExamSessions
                                            .AsNoTracking()
                                            .Where(es => es.Id == examSessionId)
                                            .SelectMany(es => es.Participants)
                                            .Where(p => p.Actions.Count > 0)
                                            .GroupJoin(dbContext.Students,
                                                        p => p.StudentId,
                                                        s => s.StudentId,
                                                        (p, s) => new { p, s })
                                            .SelectMany(
                                                t => t.s.DefaultIfEmpty(),
                                                (t, student) => new
                                                {
                                                    t.p,
                                                    student
                                                })
                                            .ToListAsync(cancellationToken);

            var result = new List<GetParticipantsDoingByExamSessionIdDto>();
            foreach (var es in examSessionQuery)
            {
                var userName = (es.student != null) ? es.student.UserName.Value : (es.p.FullName != null) ? (es.p.FullName.Value) : string.Empty;
                
                foreach (var action in es.p.Actions)
                {
                    result.Add(new GetParticipantsDoingByExamSessionIdDto(userName, action.ActionType, action.CreatedAt!.Value));
                }
            }

            return new GetParticipantsDoingByExamSessionIdResult(result);

        }
    }
}
