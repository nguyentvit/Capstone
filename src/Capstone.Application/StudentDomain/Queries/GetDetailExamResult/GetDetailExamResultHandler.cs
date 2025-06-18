using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.StudentDomain.Queries.GetDetailExamResult
{
    public class GetDetailExamResultHandler(IApplicationDbContext dbContext) : IQueryHandler<GetDetailExamResultQuery, GetDetailExamResultResult>
    {
        public async Task<GetDetailExamResultResult> Handle(GetDetailExamResultQuery query, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(query.ExamSessionId);
            var examSession = await dbContext.ExamSessions.Where(es => es.Id == examSessionId)
                                                          .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            var studentId = await dbContext.Students.AsNoTracking()
                                                    .Where(s => s.Id == UserId.Of(query.UserId))
                                                    .Select(s => s.StudentId)
                                                    .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(query.UserId);

            var participant = examSession.Participants.FirstOrDefault(p => p.StudentId != null && p.StudentId == studentId);

            if (participant == null)
                throw new BadRequestException("Bạn không nằm trong kíp thi này");

            if (!participant.IsDone.Value)
                throw new BadRequestException("Bạn chỉ có thể xem khi bài thi đã hoàn thành");

            if (!examSession.IsClosePoint.Value)
                throw new BadRequestException("Bạn chỉ có thể xem khi giảng viên đã chốt điểm");

            var examVersionId = participant.ExamVersionId;
            var examVersion = await dbContext.Exams.AsNoTracking()
                                                   .SelectMany(e => e.ExamVersions)
                                                   .Where(ev => ev.Id == examVersionId)
                                                   .FirstOrDefaultAsync(cancellationToken);

            if (examVersion == null)
                throw new ExamVersionNotFoundException(examVersionId!.Value);

            var result = new List<GetDetailExamResult>();

            foreach (var examQuestion in examVersion.Questions.OrderBy(q => q.Order))
            {
                var question = await dbContext.Questions.AsNoTracking()
                                                        .Where(q => q.Id == examQuestion.QuestionId)
                                                        .FirstOrDefaultAsync(cancellationToken);

                if (question == null)
                    throw new QuestionNotFoundException(examQuestion.QuestionId.Value);

                var answer = participant.Answers.FirstOrDefault(a => a.QuestionId == question.Id);

                result.Add(new GetDetailExamResult(QuestionExtension.ConvertToQuestionWithAnswerDto(question, (answer != null) ? answer.AnswerRaw.Value : null, (answer != null) ? answer.Score!.Value : null), (answer != null) ? answer.IsReport.Value : null, (answer != null) ? answer.IsProcess.Value : null));
            }

            return new GetDetailExamResultResult(result);
        }
    }
}
