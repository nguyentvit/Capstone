using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetReportsByExamSessionId
{
    public class GetReportsByExamSessionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetReportsByExamSessionIdQuery, GetReportsByExamSessionIdResult>
    {
        public async Task<GetReportsByExamSessionIdResult> Handle(GetReportsByExamSessionIdQuery query, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);

            var examSession = await dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.Id == examSessionId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (examSession.UserId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào");

            var result = new List<GetReportsByExamSessionIdDto>();

            if (!examSession.IsClosePoint.Value)
                return new GetReportsByExamSessionIdResult(result);

            else
            {
                var isProcess = (query.IsProcess != null ) ? IsProcess.Of(query.IsProcess.Value) : null;
                
                var groupParticipant = examSession.Participants
                                                  .Where(p => p.StudentId != null && p.Answers.Any(a => a.IsReport.Value == true))
                                                  .GroupBy(p => new { p.StudentId, p.Id })
                                                  .ToDictionary(p => p.Key, p => p.SelectMany(s => s.Answers).Where(a => (isProcess == null || a.IsProcess.Value == isProcess.Value) && a.IsReport.Value == true).ToList());

                foreach (var key in groupParticipant.Keys)
                {
                    var participant = key.Id.Value;
                    var studentId = key.StudentId;
                    var userName = await dbContext.Students.AsNoTracking()
                                                           .Where(s => s.StudentId == studentId)
                                                           .Select(s => s.UserName)
                                                           .FirstOrDefaultAsync();

                    var answerList = groupParticipant[key];

                    var list = new List<GetReportsByExamSessionIdProcess>();

                    foreach (var answer in answerList)
                    {
                        var question = await dbContext.Questions.AsNoTracking()
                                                                .Where(q => q.Id == answer.QuestionId)
                                                                .FirstOrDefaultAsync(cancellationToken);

                        if (question == null)
                            throw new QuestionNotFoundException(answer.QuestionId.Value);

                        list.Add(new GetReportsByExamSessionIdProcess(QuestionExtension.ConvertToQuestionWithAnswerDto(question, answer.AnswerRaw.Value, answer.Score!.Value), answer.IsProcess.Value));
                    }

                    result.Add(new GetReportsByExamSessionIdDto(participant, studentId!.Value, userName!.Value, list));
                }

                return new GetReportsByExamSessionIdResult(result);
            }
        }
    }
}
