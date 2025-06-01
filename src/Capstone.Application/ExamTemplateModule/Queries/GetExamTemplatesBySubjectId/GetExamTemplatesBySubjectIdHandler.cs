using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplatesBySubjectId
{
    public class GetExamTemplatesBySubjectIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamTemplatesBySubjectIdQuery, GetExamTemplatesBySubjectIdResult>
    {
        public async Task<GetExamTemplatesBySubjectIdResult> Handle(GetExamTemplatesBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var subjectId = SubjectId.Of(query.SubjectId);
            var subject = await dbContext.TeacherSubjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == subjectId)
                                         .Select(s => new { s.OwnerId })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            var userId = UserId.Of(query.UserId);

            if (subject.OwnerId != userId)
                throw new BadRequestException("Bạn không có quyền truy cập vào khung đề thi của môn học không phải của bạn");

            var examTemplatesQuery = dbContext.ExamTemplates
                                              .AsNoTracking()
                                              .Where(s => s.SubjectId == subjectId);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await examTemplatesQuery.CountAsync(cancellationToken);

            var examTemplates = await examTemplatesQuery
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .Select(s => 
                                                    new GetExamTemplatesBySubjectIdDto(
                                                        s.Id.Value, 
                                                        s.Title.Value, 
                                                        s.Description.Value, 
                                                        s.DurationInMinutes.Value))
                                                .ToListAsync(cancellationToken);

            return new GetExamTemplatesBySubjectIdResult(new PaginationResult<GetExamTemplatesBySubjectIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                examTemplates
                ));
        }
    }
}
