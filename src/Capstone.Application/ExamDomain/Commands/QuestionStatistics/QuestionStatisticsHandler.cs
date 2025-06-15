using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.ExamDomain.Commands.QuestionStatistics
{
    public class QuestionStatisticsHandler(IApplicationDbContext dbContext) : ICommandHandler<QuestionStatisticsCommand, QuestionStatisticsResult>
    {
        public async Task<QuestionStatisticsResult> Handle(QuestionStatisticsCommand command, CancellationToken cancellationToken)
        {
            var question = await dbContext.Questions
                                          .AsNoTracking()
                                          .Where(s => s.IsLastVersion == IsLastVersion.Of(true))
                                          .Select(s => new 
                                          { 
                                              s.Id,
                                              s.ChapterId,
                                              s.Type,
                                              s.Difficulty,
                                              s.Score,
                                              s.CreatedAt
                                          })
                                          .ToListAsync(cancellationToken);

            var questionIds = question.Select(s => s.Id).ToList();

            var averageDurations = await dbContext.ExamSessions
                                   .AsNoTracking()
                                   .SelectMany(es => es.Participants)
                                   .Where(p => p.IsDone == IsDone.Of(true))
                                   .SelectMany(p => p.Answers)
                                   .GroupBy(a => a.QuestionId)
                                   .ToDictionaryAsync(q => q.Key, g => g.Average(a => a.Duration.Value.TotalSeconds));

            //var totalUsageCount = await dbContext.ExamSessions
            //                                     .AsNoTracking()
                                   
            return new QuestionStatisticsResult(true);
        }
    }
}
