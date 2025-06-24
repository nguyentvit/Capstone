using Capstone.Domain.ExamModule.Entities;
using Capstone.Domain.ExamModule.Enums;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.ExamDomain.Commands.QuestionStatistics
{
    public class QuestionStatisticsHandler(IApplicationDbContext dbContext) : ICommandHandler<QuestionStatisticsCommand, QuestionStatisticsResult>
    {
        public async Task<QuestionStatisticsResult> Handle(QuestionStatisticsCommand command, CancellationToken cancellationToken)
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
                                          .Join(dbContext.TeacherSubjects,
                                                et => et.SubjectId,
                                                s => s.Id,
                                                (et, s) => new { et, s })
                                          .Where(t => t.et.Id == exam.ExamTemplateId)
                                          .Select(t => new { t.s.OwnerId, t.et.ExamTemplateSection })
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(cancellationToken);

            if (template == null || template.OwnerId.Value != command.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào tài nguyên của gói đề này");

            var chapterIds = template.ExamTemplateSection.Select(s => s.ChapterId).Distinct();

            var questions = await dbContext.Questions
                                           .AsNoTracking()
                                           .Where(q => q.IsLastVersion == IsLastVersion.Of(true) && chapterIds.Contains(q.ChapterId))
                                           .Select(q => new
                                           {
                                               q.Id,
                                               q.ChapterId,
                                               q.Difficulty,
                                               q.Type,
                                               q.CreatedAt
                                           }).ToListAsync(cancellationToken);

            var participants = await dbContext.ExamSessions
                                                .AsNoTracking()
                                                .SelectMany(es => es.Participants)
                                                .ToListAsync();

            var qsReport = participants.SelectMany(p => p.Answers).Where(a => a.IsReport.Value == true).ToList();

            var qsReportDict = qsReport.GroupBy(q => q.QuestionId).ToDictionary(g => g.Key, g => g.Count());

            var averageDurations = await dbContext.ExamSessions
                                   .AsNoTracking()
                                   .SelectMany(es => es.Participants)
                                   .Where(p => p.IsDone == IsDone.Of(true))
                                   .SelectMany(p => p.Answers)
                                   .GroupBy(a => a.QuestionId)
                                   .ToDictionaryAsync(q => q.Key, g => g.Average(a => a.Duration.Value.TotalSeconds));

            double maxDays = 90;
            double maxReports = questions.Max(q => qsReportDict.GetValueOrDefault(q.Id, 0)) + 1;

            var scoredQuestions = questions.Select(q =>
            {
                var referenceDate = q.CreatedAt ?? DateTime.MaxValue;
                var daysSinceUsed = (DateTime.UtcNow - referenceDate).TotalDays;
                var timesReported = qsReportDict.GetValueOrDefault(q.Id, 0);
                var freshness = maxDays > 0
                    ? 1.0 - Math.Min(daysSinceUsed / maxDays, 1.0)
                    : 0.0;
                double discrimination = 0.4;
                double reports = 1.0 - ((timesReported) / maxReports);
                double avgTime = averageDurations.TryGetValue(q.Id, out var t) ? t : 60;
                double timeQuality = 1 - Math.Min(Math.Abs(avgTime - 60) / 60, 1.0);
                double score = 0.25 * discrimination + 0.2  * freshness + 0.2 * timeQuality + 0.15 * reports;

                return new
                {
                    q.Id,
                    q.ChapterId,
                    q.Difficulty,
                    q.Type,
                    Score = score
                };
            }).ToList();

            var questionPool = scoredQuestions
                                .GroupBy(q => (q.ChapterId, q.Difficulty, q.Type))
                                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Score).ToList());

            for (int i = 0; i < command.Count; i++)
            {
                var usedIds = new HashSet<QuestionId>();
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

                            var selected = available
                                .Where(x => !usedIds.Contains(x.Id))
                                .Take(typeConfig.NumberOfQuestions)
                                .ToList();

                            foreach (var q in selected)
                            {
                                usedIds.Add(q.Id);
                                examQuestions.Add(ExamQuestion.Of(q.Id, order++, typeConfig.PointPerCorrect, typeConfig.PointPerInCorrect));
                            }
                        }
                    }
                }

                var code = ExamCode.Of($"EX{i + 1:D3}");

                var examVersion = ExamVersion.Of(
                    IsAnswerShuffled.Of(command.IsAnswerShuffled),
                    (OrderQuestion)command.OrderQuestion,
                    code,
                    examQuestions);

                exam.AddVersion(examVersion);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new QuestionStatisticsResult(true);
        }
    }
}
