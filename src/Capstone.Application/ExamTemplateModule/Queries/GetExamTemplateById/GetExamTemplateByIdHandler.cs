using Capstone.Domain.ExamTemplateModule.ValueObjects;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateById
{
    public class GetExamTemplateByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamTemplateByIdQuery, GetExamTemplateByIdResult>
    {
        public async Task<GetExamTemplateByIdResult> Handle(GetExamTemplateByIdQuery query, CancellationToken cancellationToken)
        {
            var examTemplateId = ExamTemplateId.Of(query.ExamTemplateId);
            var examTemplate = await dbContext.ExamTemplates
                                              .AsNoTracking()
                                              .Where(e => e.Id == examTemplateId)
                                              .Select(e => new {e.SubjectId, e.Title, e.Description, e.DurationInMinutes})
                                              .FirstOrDefaultAsync(cancellationToken);

            if (examTemplate == null)
                throw new ExamTemplateNotFoundException(examTemplateId.Value);

            var subject = await dbContext.TeacherSubjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == examTemplate.SubjectId)
                                         .Select(s => new { s.OwnerId, s.Id, s.SubjectName })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(examTemplate.SubjectId.Value);

            var userId = UserId.Of(query.UserId);
            if (subject.OwnerId != userId)
                throw new BadRequestException("Bạn không có quyền truy cập vào khung đề thi của môn học không phải của bạn");

            return new GetExamTemplateByIdResult(examTemplate.Title.Value, examTemplate.Description.Value, examTemplate.DurationInMinutes.Value, subject.Id.Value, subject.SubjectName.Value);
        }
    }
}
