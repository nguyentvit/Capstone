using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.ExamModule.Entities;
using Capstone.Domain.ExamModule.Enums;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.ExamDomain.Commands.GenerateExamVersions
{
    public record GenerateExamVersionsQuestionInput(ChapterId ChapterId, QuestionDifficulty Difficulty, QuestionType QuestionType);
    public class GenerateExamVersionsHandler(IApplicationDbContext dbContext) : ICommandHandler<GenerateExamVersionsCommand, GenerateExamVersionsResult>
    {
        public async Task<GenerateExamVersionsResult> Handle(GenerateExamVersionsCommand command, CancellationToken cancellationToken)
        {
            var examId = ExamId.Of(command.ExamId);
            var exam = await dbContext.Exams
                                      .Where(e => e.Id == examId)
                                      .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
                throw new ExamNotFoundException(examId.Value);

            if (exam.UserId.Value != command.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào tài nguyên của gói đề này");

            var template = await dbContext.ExamTemplates
                                          .Join(
                                                dbContext.TeacherSubjects,
                                                et => et.SubjectId,
                                                s => s.Id,
                                                (et, s) => new { et, s })
                                          .Where(t => t.et.Id == exam.ExamTemplateId)
                                          .Select(t => new { t.s.OwnerId, t.et.ExamTemplateSection })
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(cancellationToken);

            if (template == null)
                throw new ExamTemplateNotFoundException(exam.ExamTemplateId.Value);

            if (template.OwnerId.Value != command.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào tài nguyên của gói đề này");

            var chapterIds = template.ExamTemplateSection.Select(s => s.ChapterId).Distinct();

            var inputs = new List<GenerateExamVersionsQuestionInput>();

            foreach (var section in template.ExamTemplateSection)
            {
                foreach (var difficultySection in section.DifficultyConfigs)
                {
                    foreach (var questionType in difficultySection.QuestionTypeConfigs)
                    {
                        inputs.Add(new GenerateExamVersionsQuestionInput(section.ChapterId, difficultySection.Difficulty, questionType.Type));
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

            var errors = new List<string>();
            foreach (var section in template.ExamTemplateSection)
            {
                foreach (var difficultySection in section.DifficultyConfigs)
                {
                    foreach (var questionType in difficultySection.QuestionTypeConfigs)
                    {
                        var key = (section.ChapterId, difficultySection.Difficulty, questionType.Type);
                        groupedCounts.TryGetValue(key, out var totalOfQuestions);

                        if (questionType.NumberOfQuestions > totalOfQuestions)
                        {
                            var chapterTitle = chapters
                                .Where(c => c.Id == section.ChapterId)
                                .Select(c => c.Title.Value)
                                .FirstOrDefault() ?? string.Empty;

                            errors.Add($"Số lượng câu hỏi {questionType.Type} với độ khó {difficultySection.Difficulty} ở chương {chapterTitle} không đủ để tạo đề thi ({totalOfQuestions} < {questionType.NumberOfQuestions})");
                        }
                    }
                }
            }

            if (errors.Count != 0)
            {
                throw new BussinessException("Validation failed", errors);
            }

            var questions = await dbContext.Questions
                                                    .AsNoTracking()
                                                    .Where(q => q.ChapterId != null && chapterIds.Contains(q.ChapterId) && q.IsLastVersion == IsLastVersion.Of(true))
                                                    .Select(q => new
                                                    {
                                                        q.Id,
                                                        q.ChapterId,
                                                        q.Difficulty,
                                                        q.Type
                                                    })
                                                    .ToListAsync(cancellationToken);

            var questionPool = questions.GroupBy(q => (q.ChapterId, q.Difficulty, q.Type))
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Select(x => x.Id).ToList()
                                        );

            for (int i = 0; i < command.Count; i++)
            {
                var usedQuestionIds = new HashSet<QuestionId>();
                var examQuestions = new List<ExamQuestion>();
                int order = 1;

                foreach (var section in template.ExamTemplateSection)
                {
                    foreach (var diffConfig in section.DifficultyConfigs)
                    {
                        foreach (var typeConfig in diffConfig.QuestionTypeConfigs)
                        {
                            var key = (section.ChapterId, diffConfig.Difficulty, typeConfig.Type);

                            if (!questionPool.TryGetValue(key, out var available))
                                throw new BadRequestException("Không có câu hỏi phù hợp");

                            var unused = available.Where(q => !usedQuestionIds.Contains(q)).ToList();

                            if (unused.Count < typeConfig.NumberOfQuestions)
                                throw new BadRequestException("Không đủ câu hỏi để tạo đề");

                            var selected = unused.OrderBy(_ => Guid.NewGuid())
                                                 .Take(typeConfig.NumberOfQuestions)     
                                                 .ToList();

                            foreach (var qId in selected)
                            {
                                usedQuestionIds.Add(qId);
                                examQuestions.Add(ExamQuestion.Of(qId, order++, typeConfig.PointPerCorrect, typeConfig.PointPerInCorrect));
                            }
                        }
                    }
                }

                var code = ExamCode.Of($"EX{(i + 1):D3}");

                var examVersion = ExamVersion.Of(
                    IsAnswerShuffled.Of(command.IsAnswerShuffled), 
                    (OrderQuestion)command.OrderQuestion, 
                    code, 
                    examQuestions);

                exam.AddVersion(examVersion);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new GenerateExamVersionsResult(true);
        }
    }
}
