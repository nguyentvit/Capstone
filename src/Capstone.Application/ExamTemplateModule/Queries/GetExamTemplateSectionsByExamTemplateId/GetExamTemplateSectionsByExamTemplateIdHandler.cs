using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionsByExamTemplateId
{
    public record QuestionInput(ChapterId ChapterId, QuestionDifficulty Difficulty, QuestionType QuestionType);
    public class GetExamTemplateSectionsByExamTemplateIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetExamTemplateSectionsByExamTemplateIdQuery, GetExamTemplateSectionsByExamTemplateIdResult>
    {
        public async Task<GetExamTemplateSectionsByExamTemplateIdResult> Handle(GetExamTemplateSectionsByExamTemplateIdQuery query, CancellationToken cancellationToken)
        {
            var examTemplateId = ExamTemplateId.Of(query.ExamTemplateId);

            var examTemplate = await dbContext.ExamTemplates
                                          .Join(
                                                dbContext.TeacherSubjects,
                                                et => et.SubjectId,
                                                s => s.Id,
                                                (et, s) => new { et, s })
                                          .Where(t => t.et.Id == examTemplateId)
                                          .Select(t => new { t.s.OwnerId, t.et.ExamTemplateSection })
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(cancellationToken);

            if (examTemplate == null)
                throw new ExamTemplateNotFoundException(examTemplateId.Value);

            if (examTemplate.OwnerId.Value != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào khung đề thi của môn học không phải của bạn");

            var chapterIds = examTemplate.ExamTemplateSection.Select(s => s.ChapterId).Distinct();

            var inputs = new List<QuestionInput>();
            var numberOfQuestions = 0;
            double totalPoint = 0;

            foreach (var section in examTemplate.ExamTemplateSection)
            {
                foreach (var difficultySection in section.DifficultyConfigs)
                {
                    foreach (var questionType in difficultySection.QuestionTypeConfigs)
                    {
                        inputs.Add(new QuestionInput(section.ChapterId, difficultySection.Difficulty, questionType.Type));
                        totalPoint = totalPoint + (questionType.NumberOfQuestions * questionType.PointPerCorrect);
                        numberOfQuestions = numberOfQuestions + questionType.NumberOfQuestions;
                    }
                }
            }

            var groupedCounts = await dbContext.Questions
                                                .AsNoTracking()
                                                .Where(q => q.ChapterId != null && chapterIds.Contains(q.ChapterId) && q.IsLastVersion == IsLastVersion.Of(true))
                                                .GroupBy(q => new { q.ChapterId, q.Difficulty, q.Type })
                                                .Select(g => new
                                                {
                                                    g.Key.ChapterId,
                                                    g.Key.Difficulty,
                                                    g.Key.Type,
                                                    Count = g.Count()
                                                })
                                                .ToDictionaryAsync(
                                                    x => (x.ChapterId, x.Difficulty, x.Type),
                                                    x => x.Count,
                                                    cancellationToken
                                                );

            var chapters = await dbContext.Chapters
                                          .AsNoTracking()
                                          .Where(c => chapterIds.Contains(c.Id))
                                          .Select(c => new { c.Title, c.Id })
                                          .ToListAsync(cancellationToken);

            var result = examTemplate.ExamTemplateSection.Select(s =>
            {
                int numberOfQuestion = 0;
                double totalPointOnChapter = 0;

                var difficultyConfigs = s.DifficultyConfigs.Select(dc =>
                {
                    var questionTypeConfigs = dc.QuestionTypeConfigs.Select(q =>
                    {
                        var key = (s.ChapterId, dc.Difficulty, q.Type);
                        groupedCounts.TryGetValue(key, out var totalOfQuestions);

                        numberOfQuestion += q.NumberOfQuestions;
                        totalPointOnChapter += q.NumberOfQuestions * q.PointPerCorrect;

                        return new GetExamTemplateSectionsByExamTemplateIdQuestion(
                            q.Type,
                            q.NumberOfQuestions,
                            q.PointPerCorrect,
                            q.PointPerInCorrect,
                            totalOfQuestions
                        );
                    }).ToList();

                    return new GetExamTemplateSectionsByExamTemplateIdDifficultyDto(dc.Difficulty, questionTypeConfigs);
                }).ToList();

                var chapterTitle = chapters
                    .Where(c => c.Id == s.ChapterId)
                    .Select(c => c.Title.Value)
                    .FirstOrDefault() ?? string.Empty;

                var percentageOfQuestion = numberOfQuestions == 0 ? 0 : ((double)numberOfQuestion / numberOfQuestions) * 100;
                var pointRatio = totalPoint == 0 ? 0 : (totalPointOnChapter / totalPoint) * 100;

                return new GetExamTemplateSectionsByExamTemplateIdSectionDto(
                    s.Id.Value,
                    s.ChapterId.Value,
                    chapterTitle,
                    percentageOfQuestion,
                    pointRatio,
                    difficultyConfigs
                );
            }).ToList();


            return new GetExamTemplateSectionsByExamTemplateIdResult(result);
        }
    }
}
