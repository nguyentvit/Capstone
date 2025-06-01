using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.AddMultiChoiceQuestion
{
    public class AddMultiChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddMultiChoiceQuestionCommand, AddMultiChoiceQuestionResult>
    {
        public async Task<AddMultiChoiceQuestionResult> Handle(AddMultiChoiceQuestionCommand command, CancellationToken cancellationToken)
        {
            if (command.ChapterId != null)
            {
                var userId = UserId.Of(command.UserId);
                var chapterId = ChapterId.Of((Guid)command.ChapterId);

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

                var multiChoiceQuestion = AddMultiChoiceQuestionCommandToMultiChoiceQuestion(command);

                chapter.AddQuestionId(multiChoiceQuestion.Id);
                dbContext.MultiChoiceQuestions.Add(multiChoiceQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddMultiChoiceQuestionResult(multiChoiceQuestion.Id.Value);
            }
            else
            {
                var multiChoiceQuestion = AddMultiChoiceQuestionCommandToMultiChoiceQuestion(command);
                dbContext.MultiChoiceQuestions.Add(multiChoiceQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddMultiChoiceQuestionResult(multiChoiceQuestion.Id.Value);
            }
        }
        private static MultiChoiceQuestion AddMultiChoiceQuestionCommandToMultiChoiceQuestion(AddMultiChoiceQuestionCommand command)
        {
            var choices = command.Choices.Select(pair =>
            ChoiceMulti.Of(
                ChoiceMultiContent.Of(pair.Key),
                IsCorrect.Of(pair.Value)
            )).ToList();

            return MultiChoiceQuestion.Of(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                UserId.Of(command.UserId),
                (command.ChapterId != null) ? ChapterId.Of((Guid)command.ChapterId) : null,
                choices
                );
        }
    }
}
