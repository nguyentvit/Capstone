using Capstone.Application.Interface.Services;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvSingleChoiceQuestion
{
    public class ImportCsvSingleChoiceQuestionHandler(IApplicationDbContext dbContext, ICSVService csvService) : ICommandHandler<ImportCsvSingleChoiceQuestionCommand, ImportCsvSingleChoiceQuestionResult>
    {
        public async Task<ImportCsvSingleChoiceQuestionResult> Handle(ImportCsvSingleChoiceQuestionCommand command, CancellationToken cancellationToken)
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

            var singleChoiceQuestions = csvService.ReadCSV<ImportCsvSingleChoiceQuestionDto>(command.CsvFile.OpenReadStream());

            foreach(var scQuestion in singleChoiceQuestions)
            {
                var question = ToSingleChoiceQuestion(scQuestion, userId, chapterId);
                chapter.AddQuestionId(question.Id);

                dbContext.SingleChoiceQuestions.Add(question);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ImportCsvSingleChoiceQuestionResult(true);
        }
        private static SingleChoiceQuestion ToSingleChoiceQuestion(ImportCsvSingleChoiceQuestionDto csv, UserId userId, ChapterId chapterId)
        {
            var choiceEntities = csv.Choices.Select(c => ChoiceSingle.Of(ChoiceSingleContent.Of(c))).ToList();
            var correctAnswerId = choiceEntities[csv.CorrectAnswerIndex].Id;

            return SingleChoiceQuestion.Of(
                QuestionTitle.Of(csv.Title),
                QuestionContent.Of(csv.Content),
                (QuestionDifficulty)csv.Difficulty,
                userId,
                chapterId,
                choiceEntities,
                correctAnswerId
                );
        }
    }
}
