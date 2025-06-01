using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.ExamDomain.Queries.GetExamsBySubjectId
{
    public class GetExamsBySubjectIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamsBySubjectIdQuery, GetExamsBySubjectIdResult>
    {
        public async Task<GetExamsBySubjectIdResult> Handle(GetExamsBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var subjectId = SubjectId.Of(query.SubjectId);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var subject = await dbContext.TeacherSubjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == subjectId)
                                         .Select(s => new { s.OwnerId })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            if (subject.OwnerId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào tài nguyên của môn học này");

            var examsQuery = dbContext.Subjects
                                .AsNoTracking()
                                .Join(
                                    dbContext.ExamTemplates,
                                    s => s.Id,
                                    et => et.SubjectId,
                                    (s, et) => new { s, et })
                                .Join(
                                    dbContext.Exams,
                                    t => t.et.Id,
                                    e => e.ExamTemplateId,
                                    (t, e) => new { t.s, t.et, e })
                                .Where(t => t.s.Id == subjectId)
                                .AsNoTracking();

            var totalCount = await examsQuery.CountAsync(cancellationToken);

            var exams = await examsQuery
                                    .OrderBy(t => t.e.CreatedAt)
                                    .Select(t => new GetExamsBySubjectIdDto(t.e.Id.Value, t.e.Title.Value, t.et.Title.Value, t.e.Duration.Value, t.e.ExamVersions.Count))
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync(cancellationToken);

            return new GetExamsBySubjectIdResult(new PaginationResult<GetExamsBySubjectIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                exams
                ));
        }
    }
}
