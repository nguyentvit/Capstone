using Capstone.Domain.ExamSessionModule.Entities;
using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.JoinExamSessionFree
{
    public class JoinExamSessionFreeHandler(IApplicationDbContext dbContext) : ICommandHandler<JoinExamSessionFreeCommand, JoinExamSessionFreeResult>
    {
        public async Task<JoinExamSessionFreeResult> Handle(JoinExamSessionFreeCommand command, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(command.ExamSessionId);

            var examSession = await dbContext.ExamSessions
                                             .Where(es => es.Id == examSessionId && es.IsCodeBased == IsCodeBased.Of(false) && es.Code == ExamSessionCode.Of(command.Code))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            var dateNow = DateTime.UtcNow;
            if (examSession.StartTime.Value > dateNow || examSession.EndTime.Value < dateNow)
                throw new BadRequestException("Không nằm trong thời gian thi");

            var random = new Random();

            var examVersions = await dbContext.Exams
                                             .AsNoTracking()
                                             .Where(e => e.Id == examSession.ExamId)
                                             .Select(e => e.ExamVersions)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examVersions?.Any() != true)
                throw new BadRequestException("Không có đề thi trong kì thi này");

            var examVersion = examVersions[random.Next(examVersions.Count)];

            var participant = Participant.OfFreeCandidate(FullName.Of(command.FullName), Email.Of(command.Email));

            participant.StartExam(examVersion.Id);

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

                    return new JoinExamSessionFreeDto(dto, q.Order, q.PointPerCorrect, q.PointPerInCorrect);
                })
                .ToList();

            examSession.AddParticipant(participant);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new JoinExamSessionFreeResult(
                participant.Id.Value,
                versionInfo.Duration.Value,
                versionInfo.Code.Value,
                subjectName,
                questionDtos
                );
        }
    }
}
