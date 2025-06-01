using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsByExamSessionId
{
    public class GetParticipantsByExamSessionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetParticipantsByExamSessionIdQuery, GetParticipantsByExamSessionIdResult>
    {
        public async Task<GetParticipantsByExamSessionIdResult> Handle(GetParticipantsByExamSessionIdQuery query, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);
            var examSession = await dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .Select(es => es.UserId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (examSession.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập tài nguyên này");

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var isFree = IsFree.Of(query.IsFree);

            if (!isFree.Value)
            {
                var participantsNotFreeQuery = dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .SelectMany(es => es.Participants)
                                             .Where(p => p.IsFree == isFree)
                                             .Join(dbContext.Students,
                                                            p => p.StudentId,
                                                            s => s.StudentId,
                                                            (p, s) => new { p, s });

                var totalCountNotFree = await participantsNotFreeQuery.CountAsync(cancellationToken);
                var participantsNotFree = await participantsNotFreeQuery
                                                    .OrderBy(t => t.p.CreatedAt)
                                                    .Skip((pageIndex - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .Select(t => new GetParticipantsByExamSessionIdDto(
                                                        t.s.UserName.Value,
                                                        (t.p.StartAt != null) ? t.p.StartAt.Value : null,
                                                        (t.p.SubmittedAt != null) ? t.p.SubmittedAt.Value : null,
                                                        (t.p.StartAt != null && t.p.SubmittedAt != null) ? (t.p.SubmittedAt.Value - t.p.StartAt.Value) : null,
                                                        (t.p.Score != null) ? t.p.Score.Value : null))
                                                    .ToListAsync(cancellationToken);

                return new GetParticipantsByExamSessionIdResult(new PaginationResult<GetParticipantsByExamSessionIdDto>(
                    pageIndex, 
                    pageSize,
                    totalCountNotFree,
                    participantsNotFree
                    ));
            }

            var participantsFreeQuery = dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .SelectMany(es => es.Participants)
                                             .Where(p => p.IsFree == isFree);

            var totalCountFree = await participantsFreeQuery.CountAsync(cancellationToken);
            var participantsFree = await participantsFreeQuery
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .Select(p => new GetParticipantsByExamSessionIdDto(
                                                    (p.FullName != null) ? p.FullName.Value : null,
                                                    (p.StartAt != null) ? p.StartAt.Value : null,
                                                    (p.SubmittedAt != null) ? p.SubmittedAt.Value : null,
                                                    (p.StartAt != null && p.SubmittedAt != null) ? (p.SubmittedAt.Value - p.StartAt.Value) : null,
                                                    (p.Score != null) ? p.Score.Value : null))
                                                .ToListAsync(cancellationToken);

            return new GetParticipantsByExamSessionIdResult(new PaginationResult<GetParticipantsByExamSessionIdDto>(
                pageIndex,
                pageSize,
                totalCountFree,
                participantsFree
                ));
        }
    }
}
