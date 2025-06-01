using Capstone.Domain.ExamModule.ValueObjects;

namespace Capstone.Application.ExamDomain.Queries.GetExamVersionsByExamId
{
    public class GetExamVersionsByExamIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamVersionsByExamIdQuery, GetExamVersionsByExamIdResult>
    {
        public async Task<GetExamVersionsByExamIdResult> Handle(GetExamVersionsByExamIdQuery query, CancellationToken cancellationToken)
        {
            var examId = ExamId.Of(query.ExamId);
            var exam = await dbContext.Exams
                                      .AsNoTracking()
                                      .Where(e => e.Id == examId)
                                      .Select(e => new { e.ExamVersions, e.UserId })
                                      .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
                throw new ExamNotFoundException(examId.Value);

            if (exam.UserId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào gói đề thi này");

            var examVersions = exam.ExamVersions
                                   .OrderBy(ev => ev.Code.Value)
                                   .Select(ev => new GetExamVersionsByExamIdDto(ev.Id.Value, ev.Code.Value))
                                   .ToList();

            return new GetExamVersionsByExamIdResult(examVersions);
        }
    }
}
