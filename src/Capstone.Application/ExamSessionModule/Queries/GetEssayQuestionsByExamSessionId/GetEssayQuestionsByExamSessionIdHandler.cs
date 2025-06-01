using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamSessionModule.Queries.GetEssayQuestionsByExamSessionId
{
    public class GetEssayQuestionsByExamSessionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetEssayQuestionsByExamSessionIdQuery, GetEssayQuestionsByExamSessionIdResult>
    {
        public async Task<GetEssayQuestionsByExamSessionIdResult> Handle(GetEssayQuestionsByExamSessionIdQuery query, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);
            var examSession = await dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .Select(es => new {es.UserId})
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (examSession.UserId.Value != query.UserId)
                throw new AccessNotAllowException();

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var essayQuestionsQuery = dbContext.ExamSessions
                                               .AsNoTracking()
                                               .Where(es => es.Id == examSessionId)
                                               .SelectMany(es => es.Participants)
                                               .SelectMany(p => p.Answers, (participants, answers) => new
                                               {
                                                   Participants = participants,
                                                   Answers = answers
                                               })
                                               .Join(dbContext.Questions,
                                                     t => t.Answers.QuestionId,
                                                     q => q.Id,
                                                     (t, q) => new { t.Participants, t.Answers, q })
                                               .GroupJoin(dbContext.Students,
                                                          t => t.Participants.StudentId,
                                                          s => s.StudentId,
                                                          (t, s) => new {t.Participants, t.Answers, t.q, s})
                                               .SelectMany(
                                                    t => t.s.DefaultIfEmpty(),
                                                    (t, student) => new
                                                    {
                                                        t.Participants,
                                                        t.Answers,
                                                        t.q,
                                                        student
                                                    })
                                               .Where(t => t.q.Type == QuestionType.EssayQuestion);

            var totalCount = await essayQuestionsQuery.CountAsync(cancellationToken);
            var result = await essayQuestionsQuery
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .Select(t => new GetEssayQuestionsByExamSessionIdDto(
                                                    t.Participants.Id.Value,
                                                    (t.Participants.StudentId != null) ? t.Participants.StudentId.Value : null,
                                                    (t.Participants.FullName != null) ? t.Participants.FullName.Value : (t.student != null ? t.student.UserName.Value : string.Empty),
                                                    t.Participants.IsFree.Value,
                                                    t.q.Id.Value,
                                                    t.q.Title.Value,
                                                    t.q.Content.Value,
                                                    t.Answers.GradingStatus,
                                                    t.Answers.AnswerRaw.Value,
                                                    (t.Participants.Score != null) ? t.Participants.Score.Value : null
                                                    ))
                                                .ToListAsync(cancellationToken);

            return new GetEssayQuestionsByExamSessionIdResult(new PaginationResult<GetEssayQuestionsByExamSessionIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                result
                ));
        }
    }
}
