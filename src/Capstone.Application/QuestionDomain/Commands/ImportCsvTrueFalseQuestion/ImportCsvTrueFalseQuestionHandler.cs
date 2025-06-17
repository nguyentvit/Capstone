using Capstone.Application.Interface.Services;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvTrueFalseQuestion
{
    public class ImportCsvTrueFalseQuestionHandler(IApplicationDbContext dbContext, ICSVService csvService) : ICommandHandler<ImportCsvTrueFalseQuestionCommand, ImportCsvTrueFalseQuestionResult>
    {
        public async Task<ImportCsvTrueFalseQuestionResult> Handle(ImportCsvTrueFalseQuestionCommand command, CancellationToken cancellationToken)
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

            var trueFalseQuestionsFromCsvFile = csvService.ReadCSV<ImportCsvTrueFalseQuestionDto>(command.CsvFile.OpenReadStream());

            foreach (var tfQuestion in trueFalseQuestionsFromCsvFile)
            {
                var question = TrueFalseQuestionFromCsvFile(tfQuestion, userId, chapterId);
                chapter.AddQuestionId(question.Id);

                dbContext.TrueFalseQuestions.Add(question);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ImportCsvTrueFalseQuestionResult(true);
        }
        private static TrueFalseQuestion TrueFalseQuestionFromCsvFile(ImportCsvTrueFalseQuestionDto csvFile, UserId userId, ChapterId chapterId)
        {
            return TrueFalseQuestion.Of(
                QuestionTitle.Of(csvFile.Title),
                QuestionContent.Of(csvFile.Content),
                (QuestionDifficulty)csvFile.Difficulty,
                userId,
                chapterId,
                IsTrueAnswer.Of(csvFile.IsTrueAnswer)
                );
        }
    }
}
