using Capstone.Domain.ExamTemplateModule.ValueObjects;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionById
{
    public class GetExamTemplateSectionByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamTemplateSectionByIdQuery, GetExamTemplateSectionByIdResult>
    {
        public async Task<GetExamTemplateSectionByIdResult> Handle(GetExamTemplateSectionByIdQuery query, CancellationToken cancellationToken)
        {
            var sectionId = ExamTemplateSectionId.Of(query.SectionId);
            var section = await dbContext.ExamTemplates
                                         .AsNoTracking()
                                         .Where(e => e.ExamTemplateSection.Any(s => s.Id == sectionId))
                                         .Select(e => new { e.SubjectId, Section = e.ExamTemplateSection.FirstOrDefault(s => s.Id == sectionId) })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (section == null || section.Section == null)
                throw new ExamTemplateSectionNotFoundException(sectionId.Value);

            var subject = await dbContext.TeacherSubjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == section.SubjectId)
                                         .Select(s => new { s.OwnerId })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(section.SubjectId.Value);

            var userId = UserId.Of(query.UserId);

            if (subject.OwnerId != userId)
                throw new BadRequestException("Bạn không có quyền truy cập vào khung đề thi của môn học không phải của bạn");

            var chapter = await dbContext.Chapters
                                         .AsNoTracking()
                                         .Where(c => c.Id == section.Section.ChapterId)
                                         .Select(c => c.Title)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (chapter == null)
                throw new ChapterNotFoundException(section.Section.ChapterId.Value);

            var difficultyConfigs = section.Section.DifficultyConfigs.Select(dc =>
            {
                var questionTypeConfigs = dc.QuestionTypeConfigs.Select(q => new GetExamTemplateSectionByIdQuestionDto(q.Type, q.NumberOfQuestions, q.PointPerCorrect, q.PointPerInCorrect)).ToList();
                return new GetExamTemplateSectionByIdDifficultyDto(dc.Difficulty, questionTypeConfigs);
            }).ToList();
            return new GetExamTemplateSectionByIdResult(section.Section.ChapterId.Value, chapter.Value, difficultyConfigs);
        }
    }
}
