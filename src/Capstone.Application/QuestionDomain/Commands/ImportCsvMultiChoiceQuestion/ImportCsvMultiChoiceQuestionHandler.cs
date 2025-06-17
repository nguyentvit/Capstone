using Capstone.Application.Interface.Services;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvMultiChoiceQuestion
{
    public class ImportCsvMultiChoiceQuestionHandler(IApplicationDbContext dbContext, ICSVService csvService) : ICommandHandler<ImportCsvMultiChoiceQuestionCommand, ImportCsvMultiChoiceQuestionResult>
    {
        public async Task<ImportCsvMultiChoiceQuestionResult> Handle(ImportCsvMultiChoiceQuestionCommand command, CancellationToken cancellationToken)
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

            var multiChoiceQuestions = csvService.ReadCSV<ImportCsvMultiChoiceQuestionDto>(command.Csv.OpenReadStream());

            foreach (var mc in multiChoiceQuestions)
            {
                var question = ToMultiChoice(mc, userId, chapterId);
                chapter.AddQuestionId(question.Id);

                dbContext.MultiChoiceQuestions.Add(question);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new ImportCsvMultiChoiceQuestionResult(true);
        }
        private static MultiChoiceQuestion ToMultiChoice(ImportCsvMultiChoiceQuestionDto csv, UserId userId, ChapterId chapterId)
        {
            var choices = csv.Choices.Select(pair => ChoiceMulti.Of(ChoiceMultiContent.Of(pair.Key), IsCorrect.Of(pair.Value))).ToList();

            return MultiChoiceQuestion.Of(
                QuestionTitle.Of(csv.Title),
                QuestionContent.Of(csv.Content),
                (QuestionDifficulty)csv.Difficulty,
                userId,
                chapterId,
                choices
                );
        }
    }
}
