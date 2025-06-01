using Capstone.Domain.ExamModule.ValueObjects;

namespace Capstone.Application.ExamDomain.Queries.GetExamVersionById
{
    public class GetExamVersionByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamVersionByIdQuery, GetExamVersionByIdResult>
    {
        public async Task<GetExamVersionByIdResult> Handle(GetExamVersionByIdQuery query, CancellationToken cancellationToken)
        {
            var examVersionId = ExamVersionId.Of(query.ExamVersionId);

            var versionInfo = await dbContext.Exams
                .AsNoTracking()
                .SelectMany(e => e.ExamVersions, (exam, version) => new
                {
                    Exam = exam,
                    Version = version
                })
                .Where(x => x.Version.Id == examVersionId)
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
                throw new ExamVersionNotFoundException(examVersionId.Value);

            if (versionInfo.UserId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào mã đề này");

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

                    return new GetExamVersionByIdQuestion(dto, q.Order, q.PointPerCorrect, q.PointPerInCorrect);
                })
                .ToList();

            return new GetExamVersionByIdResult(
                versionInfo.Duration.Value,
                versionInfo.Code.Value,
                subjectName,
                questionDtos
                );
        }
    }
}
