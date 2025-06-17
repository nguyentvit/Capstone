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
            throw new NotImplementedException();
        }
    }
}
