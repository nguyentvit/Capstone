using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Queries.GetParticipantsResult
{
    public class GetParticipantsResultHandler(IApplicationDbContext dbContext) : IQueryHandler<GetParticipantsResultQuery, GetParticipantsResultResult>
    {
        public async Task<GetParticipantsResultResult> Handle(GetParticipantsResultQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);

            var es = await dbContext.ExamSessions
                                    .AsNoTracking()
                                    .Where(es => es.UserId == userId && es.Id == ExamSessionId.Of(query.ExamSessionId))
                                    .FirstOrDefaultAsync(cancellationToken);

            if (es == null)
                throw new BadRequestException("Không tìm thấy");

            var examSession = await dbContext.ExamSessions
                                             .AsNoTracking()
                                             .Where(es => es.UserId == userId && es.Id == ExamSessionId.Of(query.ExamSessionId))
                                             .SelectMany(es => es.Participants)
                                             .Where(p => p.IsDone == IsDone.Of(true))
                                             .ToListAsync();

            double totalScore = 0;

            var examVersion = await dbContext.Exams
                                      .AsNoTracking()
                                      .Where(e => e.Id == es.ExamId)
                                      .Select(e => e.ExamVersions.First())
                                      .FirstOrDefaultAsync(cancellationToken);

            if (examVersion == null)
                throw new BadRequestException("không tìm thấy đề nào");

            foreach (var question in examVersion.Questions)
            {
                totalScore += question.PointPerCorrect;
            }

            var result = new List<GetParticipantsResultDto>();

            foreach (var participant in examSession)
            {
                var studentId = (participant.StudentId != null) ? participant.StudentId.Value : null;
                var userName = string.Empty;
                if (studentId != null)
                {
                    var student = await dbContext.Students.AsNoTracking().Where(s => s.StudentId == StudentId.Of(studentId)).Select(s => s.UserName).FirstOrDefaultAsync();
                    if (student != null)
                    {
                        userName = student.Value;
                    }
                }
                else
                {
                    userName = (participant.FullName != null) ? participant.FullName.Value : string.Empty;
                }
                var isFree = participant.IsFree.Value;
                double score = 0;
                foreach (var answer in participant.Answers)
                {
                    if (answer.Score != null)
                    {
                        score += answer.Score.Value;
                    }
                }
                result.Add(new GetParticipantsResultDto(studentId, userName, score, totalScore, isFree));
            }
            
            return new GetParticipantsResultResult(result);
        }
    }
}
