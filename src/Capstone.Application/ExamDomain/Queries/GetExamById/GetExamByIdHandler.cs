using Capstone.Domain.ExamModule.ValueObjects;

namespace Capstone.Application.ExamDomain.Queries.GetExamById
{
    public class GetExamByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamByIdQuery, GetExamByIdResult>
    {
        public async Task<GetExamByIdResult> Handle(GetExamByIdQuery query, CancellationToken cancellationToken)
        {
            var examId = ExamId.Of(query.ExamId);
            var exam = await dbContext.Exams
                                      .AsNoTracking()
                                      .Where(e => e.Id == examId)
                                      .Select(e => new { e.Title, e.Duration, e.UserId })
                                      .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
                throw new ExamNotFoundException(examId.Value);

            if (exam.UserId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào gói đề kiểm tra này");

            return new GetExamByIdResult(exam.Title.Value, exam.Duration.Value);
        }
    }
}
