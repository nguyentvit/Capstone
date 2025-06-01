using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.StudentDomain.Commands.JoinExamSession
{
    public class JoinExamSessionHandler(IApplicationDbContext dbContext) : ICommandHandler<JoinExamSessionCommand, JoinExamSessionResult>
    {
        public async Task<JoinExamSessionResult> Handle(JoinExamSessionCommand command, CancellationToken cancellationToken)
        {
            var studentId = await dbContext.Students
                                           .AsNoTracking()
                                           .Where(s => s.Id == UserId.Of(command.UserId))
                                           .Select(s => s.StudentId)
                                           .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(command.UserId);

            var examSession = await dbContext.ExamSessions
                                             .SelectMany(e => e.Participants, (es, participants) => new
                                             {
                                                 ExamSession = es,
                                                 Participants = participants
                                             })
                                             .Where(t => t.Participants.StudentId != null && t.Participants.StudentId == studentId && t.ExamSession.Id == ExamSessionId.Of(command.ExamSessionId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            if (examSession.Participants.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể tham gia");

            var dateNow = DateTime.UtcNow;
            if (examSession.ExamSession.StartTime.Value > dateNow || examSession.ExamSession.EndTime.Value < dateNow)
                throw new BadRequestException("Không nằm trong thời gian thi");

            var random = new Random();

            var examVersions = await dbContext.Exams
                                             .AsNoTracking()
                                             .Where(e => e.Id == examSession.ExamSession.ExamId)
                                             .Select(e => e.ExamVersions)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examVersions?.Any() != true)
                throw new BadRequestException("Không có đề thi trong kì thi này");

            var examVersion = examVersions[random.Next(examVersions.Count)];

            examSession.Participants.StartExam(examVersion.Id);

            var versionInfo = await dbContext.Exams
                .AsNoTracking()
                .SelectMany(e => e.ExamVersions, (exam, version) => new
                {
                    Exam = exam,
                    Version = version
                })
                .Where(x => x.Version.Id == examVersion.Id)
                .Select(x => new
                {
                    x.Version,
                    x.Exam.UserId,
                    x.Exam.Duration,
                    x.Exam.ExamTemplateId,
                    x.Version.Code,
                    x.Version.Questions
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (versionInfo == null)
                throw new ExamVersionNotFoundException(examVersion.Id.Value);

            var subjectName = await dbContext.ExamTemplates
                .Where(et => et.Id == versionInfo.ExamTemplateId)
                .Join(dbContext.Subjects,
                    et => et.SubjectId,
                    s => s.Id,
                    (et, s) => s.SubjectName.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(subjectName))
                throw new ExamTemplateNotFoundException(versionInfo.ExamTemplateId.Value);

            var questionIds = versionInfo.Questions.Select(q => q.QuestionId).ToList();

            var questionMap = await dbContext.Questions
                .AsNoTracking()
                .Where(q => questionIds.Contains(q.Id))
                .Select(q => new
                {
                    q.Id,
                    Dto = QuestionExtension.ConvertToQuestionDto(q)
                })
                .ToDictionaryAsync(x => x.Id, x => x.Dto, cancellationToken);

            if (questionMap.Count != versionInfo.Questions.Count)
                throw new BadRequestException("Không tìm thấy đủ câu hỏi của mã đề");

            var questionDtos = versionInfo.Questions
                .OrderBy(q => q.Order)
                .Select(q =>
                {
                    if (!questionMap.TryGetValue(q.QuestionId, out var dto))
                        throw new BadRequestException($"Không tìm thấy câu hỏi ID = {q.QuestionId.Value}");

                    return new JoinExamSessionDto(dto, q.Order, q.PointPerCorrect, q.PointPerInCorrect);
                })
                .ToList();

            await dbContext.SaveChangesAsync(cancellationToken);

            return new JoinExamSessionResult(
                versionInfo.Duration.Value,
                versionInfo.Code.Value,
                subjectName,
                questionDtos
                );
        }
    }
}
