using Capstone.Application.Interface.Services;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvEssayQuestion
{
    public class ImportCsvEssayQuestionHandler(IApplicationDbContext dbContext, ICSVService csvService) : ICommandHandler<ImportCsvEssayQuestionCommand, ImportCsvEssayQuestionResult>
    {
        public async Task<ImportCsvEssayQuestionResult> Handle(ImportCsvEssayQuestionCommand command, CancellationToken cancellationToken)
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

            var essayQuestions = csvService.ReadCSV<ImportCsvEssayQuestionDto>(command.CsvFile.OpenReadStream());

            foreach (var eq in essayQuestions)
            {
                var question = ToEssayQuestion(eq, userId, chapterId);
                chapter.AddQuestionId(question.Id);

                dbContext.EssayQuestions.Add(question);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new ImportCsvEssayQuestionResult(true);
        }
        private static EssayQuestion ToEssayQuestion(ImportCsvEssayQuestionDto csv, UserId userId, ChapterId chapterId)
        {
            return EssayQuestion.Of(
                QuestionTitle.Of(csv.Title),
                QuestionContent.Of(csv.Content),
                (QuestionDifficulty)csv.Difficulty,
                userId,
                chapterId
                );
        }
    }
}
