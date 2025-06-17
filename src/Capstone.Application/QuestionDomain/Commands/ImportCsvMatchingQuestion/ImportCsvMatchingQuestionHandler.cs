using Capstone.Application.Interface.Services;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Entities;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvMatchingQuestion
{
    public class ImportCsvMatchingQuestionHandler(IApplicationDbContext dbContext, ICSVService csvService) : ICommandHandler<ImportCsvMatchingQuestionCommand, ImportCsvMatchingQuestionResult>
    {
        public async Task<ImportCsvMatchingQuestionResult> Handle(ImportCsvMatchingQuestionCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.UserId);
            var chapterId = ChapterId.Of(command.ChapterId);

            var chapter = await dbContext.Chapters
                                         .Where(c => c.Id == chapterId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (chapter == null)
                throw new ChapterNotFoundException(chapterId.Value);

            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == chapter.SubjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(chapter.SubjectId.Value);

            SubjectExtention.CheckRole(subject, userId, command.Role);

            var matchingQuestions = csvService.ReadCSV<ImportCsvMatchingQuestionDto>(command.Csv.OpenReadStream());

            foreach (var mq in matchingQuestions)
            {
                var question = ToMatchingQuestion(mq, userId, chapterId);
                chapter.AddQuestionId(question.Id);

                dbContext.MatchingQuestions.Add(question);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ImportCsvMatchingQuestionResult(true);
        }
        private static MatchingQuestion ToMatchingQuestion(ImportCsvMatchingQuestionDto csv, UserId userId, ChapterId chapterId)
        {
            var pairs = csv.MatchingPairs
                               .Select(pair => MatchingPair.Of(MatchingPairContentLeft.Of(pair.Key), MatchingPairContentRight.Of(pair.Value)))
            .ToList();

            return MatchingQuestion.Of(
                QuestionTitle.Of(csv.Title),
                QuestionContent.Of(csv.Content),
                (QuestionDifficulty)csv.Difficulty,
                userId,
                chapterId,
                pairs
                );
        }
    }
}
