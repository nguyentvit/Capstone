using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetExamSessionById
{
    public class GetExamSessionByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamSessionByIdQuery, GetExamSessionByIdResult>
    {
        public async Task<GetExamSessionByIdResult> Handle(GetExamSessionByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);

            var examSession = await dbContext.ExamSessions.AsNoTracking()
                                                               .Join(dbContext.Exams,
                                                                    es => es.ExamId,
                                                                    e => e.Id,
                                                                    (es, e) => new { es, e })
                                                               .Where(t => t.es.Id == examSessionId && t.e.UserId == userId)
                                                               .Select(t => new GetExamSessionByIdResult(
                                                                   t.es.Name.Value,
                                                                   t.es.StartTime.Value,
                                                                   t.es.EndTime.Value,
                                                                   t.es.Duration.Value,
                                                                   t.es.IsCodeBased.Value,
                                                                   (t.es.Code != null) ? t.es.Code.Value : null,
                                                                   t.e.Id.Value,
                                                                   t.e.Title.Value,
                                                                   t.es.IsDone.Value,
                                                                   t.es.IsClosePoint.Value))
                                                               .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            return examSession;
        }
    }
}
