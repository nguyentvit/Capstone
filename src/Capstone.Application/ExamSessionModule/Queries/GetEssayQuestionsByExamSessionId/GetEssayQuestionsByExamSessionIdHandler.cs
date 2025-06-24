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

            var essayQuestions1 = await dbContext.ExamSessions
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
                                               .Where(t => t.q.Type == QuestionType.EssayQuestion && t.Participants.IsDone == IsDone.Of(true))
                                               .ToListAsync();

            var essayQuestions = await dbContext.ExamSessions
                                               .AsNoTracking()
                                               .Where(es => es.Id == examSessionId)
                                               .SelectMany(es => es.Participants)
                                               .SelectMany(p => p.Answers, (participants, answers) => new
                                               {
                                                   Participant = participants,
                                                   Answers = answers
                                               })
                                               .Join(dbContext.Questions,
                                                     t => t.Answers.QuestionId,
                                                     q => q.Id,
                                                     (t, q) => new { t.Participant, t.Answers, q })
                                               .Where(t => t.q.Type == QuestionType.EssayQuestion && t.Participant.IsDone == IsDone.Of(true))
                                               .ToListAsync();

            var groupParticipant = essayQuestions.GroupBy(p => p.Participant.Id.Value)
                                                 .ToDictionary(p => p.Key, p => p.Select(a => new { a.q, a.Answers }).ToList());

            var result = new List<GetEssayQuestionsByExamSessionIdDto>();

            foreach (var participantId in groupParticipant.Keys)
            {
                string userName = string.Empty;
                var participant = essayQuestions.Where(t => t.Participant.Id.Value == participantId).Select(t => t.Participant).FirstOrDefault();
                if (participant!.IsFree.Value)
                {
                    userName = participant.FullName!.Value;
                }
                else if (participant.StudentId != null)
                {
                    var un = await dbContext.Students.AsNoTracking()
                                                       .Where(s => s.StudentId == participant.StudentId)
                                                       .Select(s => s.UserName)
                                                       .FirstOrDefaultAsync();

                    userName = (un != null) ? un.Value : string.Empty;
                }

                var value = groupParticipant[participantId];
                var b = new List<GetEssayQuestionsByExamSessionIdAnswer>();
                foreach (var t in value)
                {
                    var answerTrueFalse = JsonConvert.DeserializeObject<EssayAnswer>(t.Answers.AnswerRaw.Value);
                    var an = new GetEssayQuestionsByExamSessionIdAnswer(t.q.Id.Value, t.q.Title.Value, t.q.Content.Value, t.Answers.GradingStatus, answerTrueFalse!.Answer, (t.Answers.Score != null) ? t.Answers.Score.Value : null);
                    b.Add(an);
                }
                result.Add(new GetEssayQuestionsByExamSessionIdDto(participant.Id.Value, (participant.StudentId != null) ? participant.StudentId.Value : null, userName, participant.IsFree.Value, b));
            }

            return new GetEssayQuestionsByExamSessionIdResult(result);

            

            //var totalCount = await essayQuestionsQuery.CountAsync(cancellationToken);

            //var result = await essayQuestionsQuery
            //                                    .Skip((pageIndex - 1) * pageSize)
            //                                    .Take(pageSize)
            //                                    .Select(t => new GetEssayQuestionsByExamSessionIdDto(
            //                                        t.Participants.Id.Value,
            //                                        (t.Participants.StudentId != null) ? t.Participants.StudentId.Value : null,
            //                                        (t.Participants.FullName != null) ? t.Participants.FullName.Value : (t.student != null ? t.student.UserName.Value : string.Empty),
            //                                        t.Participants.IsFree.Value,
            //                                        t.q.Id.Value,
            //                                        t.q.Title.Value,
            //                                        t.q.Content.Value,
            //                                        t.Answers.GradingStatus,
            //                                        t.Answers.AnswerRaw.Value,
            //                                        (t.Answers.Score != null) ? t.Answers.Score.Value : null
            //                                        ))
            //                                    .ToListAsync(cancellationToken);

            //var rs = result.Select(t =>
            //{
            //    var answerTrueFalse = JsonConvert.DeserializeObject<EssayAnswer>(t.Answer);
            //    return new GetEssayQuestionsByExamSessionIdDto(t.ParticipantId, t.StudentId, t.UserName, t.IsFree, t.QuestionId, t.QuestionTitle, t.QuestionContent, t.GradingStatus, answerTrueFalse!.Answer, t.Score);
            //}).ToList();

            //return new GetEssayQuestionsByExamSessionIdResult(new PaginationResult<GetEssayQuestionsByExamSessionIdDto>(
            //    pageIndex,
            //    pageSize,
            //    totalCount,
            //    rs
            //    ));
        }
    }
}
